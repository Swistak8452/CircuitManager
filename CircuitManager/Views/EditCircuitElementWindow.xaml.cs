using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using CircuitManager.Data;
using CircuitManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CircuitManager.Views;

public partial class EditCircuitElementWindow : Window
{
    private readonly int? _elementId;
    private readonly AppDbContext _db = new();
    private CircuitElement _element = new();

    // Flagi do logiki autopodpowiedzi
    private bool _nameManuallyEdited = false;
    private bool _isProgrammaticNameUpdate = false;
    private string? _lastAutoName = null;

    public EditCircuitElementWindow(int? elementId = null)
    {
        InitializeComponent();
        _elementId = elementId;
        LoadData();
    }

    private void LoadData()
    {
        // 1) MachineTypes z bazy
        MachineTypeCombo.ItemsSource = _db.MachineTypes
                                          .OrderBy(t => t.Id)
                                          .ToList();

        // 2) Components z bazy
        ComponentsList.ItemsSource = _db.Components
                                        .OrderBy(c => c.Label)
                                        .ToList();

        // 3) Jeżeli edycja — wczytaj element (z relacjami)
        if (_elementId != null)
        {
            _element = _db.CircuitElements
                .Include(e => e.ComponentList)
                .Include(e => e.NextCircuitElement)
                .Include(e => e.MachineType)
                .First(e => e.Id == _elementId);
        }

        // 4) Next element: wszystkie poza samym sobą + "None"
        var allElements = _db.CircuitElements.OrderBy(e => e.Name).ToList();
        var filtered = _elementId == null ? allElements : allElements.Where(e => e.Id != _elementId).ToList();
        NextElementCombo.ItemsSource = new[] { "None" }.Concat(filtered.Select(e => e.Name));

        // 5) Edycja w UI
        if (_elementId != null)
        {
            NameBox.Text = _element.Name;
            MachineTypeCombo.SelectedValue = _element.MachineTypeId;
            NextElementCombo.SelectedItem = _element.NextCircuitElement?.Name ?? "None";

            foreach (var comp in _element.ComponentList)
                ComponentsList.SelectedItems.Add(comp);

            // Przy edycji nie autouzupelniamy — user już ma nazwę wprowadzoną ręcznie
            _nameManuallyEdited = true;
        }
        else
        {
            // Przy dodawaniu: brak nazwy => pozwól na auto-sugestię po wyborze typu
            _nameManuallyEdited = false;
        }
    }

    // Użytkownik zaczął pisać nazwę – nie nadpisujmy jej automatycznie
    private void NameBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        if (_isProgrammaticNameUpdate) return;
        _nameManuallyEdited = true;
    }

    // Po zmianie MachineType — jeśli user nie wpisał sam, zaproponuj Label+liczba (TP1, TP2, ...)
    private void MachineTypeCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (_elementId != null) return; // tylko podczas dodawania nowego
        if (_nameManuallyEdited && !string.IsNullOrWhiteSpace(NameBox.Text)) return;

        if (MachineTypeCombo.SelectedItem is MachineType mt)
        {
            var suggested = GenerateNextName(mt.Label);
            _isProgrammaticNameUpdate = true;
            try
            {
                NameBox.Text = suggested;
                _lastAutoName = suggested;
            }
            finally
            {
                _isProgrammaticNameUpdate = false;
            }
            // do tej pory user nic nie pisał – nadal pozwalamy nadpisać przy kolejnej zmianie typu
            _nameManuallyEdited = false;
        }
    }

    // Logika wyznaczania kolejnego numeru
    private string GenerateNextName(string label)
    {
        // Pobierz istniejące nazwy zaczynające się od label (np. "TP")
        var names = _db.CircuitElements
                       .Where(e => e.Name.StartsWith(label))
                       .Select(e => e.Name)
                       .ToList();

        // Wzorzec: "TP" [spacje opcjonalnie] <liczba>
        var rx = new Regex(@"^" + Regex.Escape(label) + @"\s*(\d+)$", RegexOptions.IgnoreCase);

        int max = 0;
        foreach (var n in names)
        {
            var m = rx.Match(n);
            if (m.Success && int.TryParse(m.Groups[1].Value, out var num))
                if (num > max) max = num;
        }

        return $"{label}{max + 1}";
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        _element.Name = NameBox.Text.Trim();

        if (MachineTypeCombo.SelectedValue is not int mtId)
        {
            MessageBox.Show("Wybierz Machine Type.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        _element.MachineTypeId = mtId;

        _element.ComponentList.Clear();
        foreach (Component selected in ComponentsList.SelectedItems)
            _element.ComponentList.Add(selected);

        var selectedNext = NextElementCombo.SelectedItem?.ToString();
        _element.NextCircuitElement = selectedNext == "None"
            ? null
            : _db.CircuitElements.FirstOrDefault(e => e.Name == selectedNext);

        if (_elementId == null)
            _db.CircuitElements.Add(_element);
        else
            _db.CircuitElements.Update(_element);

        _db.SaveChanges();
        DialogResult = true;
        Close();
    }
}
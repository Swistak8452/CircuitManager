using System.Collections.Generic;
using System.Linq;
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


    public EditCircuitElementWindow(int? elementId = null)
    {
        InitializeComponent();
        _elementId = elementId;
        LoadData();
    }


    private void LoadData()
    {
        MachineTypeCombo.ItemsSource = Enum.GetValues(typeof(MachineType));
        ComponentsList.ItemsSource = _db.Components.ToList();


        if (_elementId != null)
        {
            _element = _db.CircuitElements
                .Include(e => e.ComponentList)
                .Include(e => e.NextCircuitElement)
                .First(e => e.Id == _elementId);
        }


        var allElements = _db.CircuitElements.ToList();
        var filteredElements = _elementId == null
            ? allElements
            : allElements.Where(e => e.Id != _elementId).ToList();


        NextElementCombo.ItemsSource = new[] { "None" }.Concat(filteredElements.Select(e => e.Name));


        if (_elementId != null)
        {
            NameBox.Text = _element.Name;
            MachineTypeCombo.SelectedItem = _element.MachineType;
            NextElementCombo.SelectedItem = _element.NextCircuitElement?.Name ?? "None";


            foreach (var comp in _element.ComponentList)
            {
                ComponentsList.SelectedItems.Add(comp);
            }
        }
    }


    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }


    private void Save_Click(object sender, RoutedEventArgs e)
    {
        _element.Name = NameBox.Text.Trim();
        _element.MachineType = (MachineType)MachineTypeCombo.SelectedItem!;
        _element.ComponentList.Clear();


        foreach (Component selected in ComponentsList.SelectedItems)
        {
            _element.ComponentList.Add(selected);
        }


        string selectedNext = NextElementCombo.SelectedItem?.ToString();
        _element.NextCircuitElement = _db.CircuitElements.FirstOrDefault(e => e.Name == selectedNext);


        if (_elementId == null)
            _db.CircuitElements.Add(_element);
        else
            _db.CircuitElements.Update(_element);


        _db.SaveChanges();
        DialogResult = true;
        Close();
    }
}
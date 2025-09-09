using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows;
using CircuitManager.Data;
using CircuitManager.Models;
using CircuitManager.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace CircuitManager.Views;

public partial class ExportCircuitDialog : Window
{
        private readonly AppDbContext _db = new();

    public ExportCircuitDialog()
    {
        InitializeComponent();

        // dociągamy relacje, bo będą potrzebne do tagów i rekurencji
        CircuitList.ItemsSource = _db.CircuitElements
            .Include(e => e.MachineType)
            .Include(e => e.ComponentList)
            .Include(e => e.NextCircuitElement)
            .ToList();
    }

    // ======= Export 1: pełny JSON wybranych elementów (z tagami przy komponentach) =======
    private void Export_Selected_Click(object sender, RoutedEventArgs e)
    {
        var selected = CircuitList.SelectedItems.Cast<CircuitElement>().ToList();
        var exportList = selected
            .Select(e => MapElementWithTags(e))
            .Where(dto => dto != null)
            .ToList();

        var json = JsonSerializer.Serialize(exportList, new JsonSerializerOptions { WriteIndented = true });
        new JsonViewerDialog(json).ShowDialog();
        Close();
    }

    // ======= Export 2: płaskie listy inputs/outputs z wybranych elementów =======
    private void Export_IO_Click(object sender, RoutedEventArgs e)
    {
        var selected = CircuitList.SelectedItems.Cast<CircuitElement>().ToList();

        var inputs = new List<string>();
        var outputs = new List<string>();

        foreach (var ce in selected)
        {
            BuildTagsForElement(ce, out var inTags, out var outTags);
            inputs.AddRange(inTags);
            outputs.AddRange(outTags);
        }

        var payload = new { inputs, outputs };
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        new JsonViewerDialog(json).ShowDialog();
        Close();
    }

    // ======= Mapowanie elementu do DTO z tagami w ComponentList =======
    private CircuitElementExportDto? MapElementWithTags(CircuitElement element, HashSet<int>? visited = null)
    {
        visited ??= new HashSet<int>();
        if (!visited.Add(element.Id)) return null; // antycykliczne

        var comps = BuildComponentsWithTags(element);

        return new CircuitElementExportDto
        {
            Name = element.Name,
            MachineType = new MachineTypeDto
            {
                Label = element.MachineType?.Label ?? "",
                Name  = element.MachineType?.Name  ?? ""
            },
            ComponentList = comps.ToList(),
            NextCircuitElement = element.NextCircuitElement != null
                ? MapElementWithTags(EnsureLoaded(element.NextCircuitElement), visited)
                : null
        };
    }

    // ======= Wyliczanie tagów dla komponentów danego elementu =======
    private IEnumerable<ComponentExportDto> BuildComponentsWithTags(CircuitElement element)
    {
        // Numeracja osobno dla każdego labela (prefiksu), np. B1..Bn, Q1..Qn
        var groups = element.ComponentList
            .OrderBy(c => c.Label)
            .GroupBy(c => c.Label);

        foreach (var g in groups)
        {
            int idx = 1;
            foreach (var c in g)
            {
                var tag = $"{element.Name}_{g.Key}{idx}";
                yield return new ComponentExportDto
                {
                    Name      = c.Name,
                    Label     = c.Label,
                    Direction = c.Direction.ToString(), // "Input"/"Output"
                    Index     = idx,
                    Tag       = tag
                };
                idx++;
            }
        }
    }

    // ======= Budowanie listy tagów Input/Output (rekurencyjnie przez łańcuch Next) =======
    private void BuildTagsForElement(CircuitElement element, out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>();
        outputs = new List<string>();

        foreach (var c in BuildComponentsWithTags(element))
        {
            if (c.Direction == nameof(IODirection.Input)) inputs.Add(c.Tag);
            else                                          outputs.Add(c.Tag);
        }

        if (element.NextCircuitElement != null)
        {
            var next = EnsureLoaded(element.NextCircuitElement);
            BuildTagsForElement(next, out var i2, out var o2);
            inputs.AddRange(i2);
            outputs.AddRange(o2);
        }
    }

    // ======= Dociągnięcie relacji, jeśli niezaładowane =======
    private CircuitElement EnsureLoaded(CircuitElement e)
    {
        if (e.MachineType == null || e.ComponentList == null)
        {
            e = _db.CircuitElements
                .Include(x => x.MachineType)
                .Include(x => x.ComponentList)
                .Include(x => x.NextCircuitElement)
                .First(x => x.Id == e.Id);
        }
        return e;
    }

}
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
        CircuitList.ItemsSource = _db.CircuitElements
            .Include(e => e.ComponentList)
            .Include(e => e.NextCircuitElement)
            .ToList();
    }


    private void Export_Click(object sender, RoutedEventArgs e)
    {
        var selected = CircuitList.SelectedItems.Cast<CircuitElement>().ToList();
        var exportList = selected.Select(e => MapElement(e)).Where(dto => dto != null).ToList();


        var json = JsonSerializer.Serialize(exportList, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var viewer = new JsonViewerDialog(json);
        viewer.ShowDialog();
        Close();
    }




    private CircuitElementExportDto MapElement(CircuitElement element, HashSet<int>? visited = null)
    {
        visited ??= new HashSet<int>();
        if (!visited.Add(element.Id)) return null!; // zapobiega nieskończonej rekurencji

        return new CircuitElementExportDto
        {
            Name = element.Name,
            MachineType = element.MachineType,
            ComponentList = element.ComponentList.Select(c => new ComponentExportDto
            {
                Name = c.Name,
                Shortcut = c.Shortcut
            }).ToList(),
            NextCircuitElement = element.NextCircuitElement != null
                ? MapElement(element.NextCircuitElement, visited)
                : null
        };
    }

}
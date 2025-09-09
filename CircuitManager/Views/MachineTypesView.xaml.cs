using System.Windows;
using System.Windows.Controls;
using CircuitManager.Data;
using CircuitManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CircuitManager.Views;

public partial class MachineTypesView : UserControl
{
    public MachineTypesView()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        using var db = new AppDbContext();
        Grid.ItemsSource = db.MachineTypes
            .OrderBy(t => t.Id)
            .ToList();
    }

    private void Refresh_Click(object sender, RoutedEventArgs e) => LoadData();

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new EditMachineTypeWindow();
        if (dlg.ShowDialog() == true) LoadData();
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button b && b.Tag is MachineType mt)
        {
            var dlg = new EditMachineTypeWindow(mt.Id);
            if (dlg.ShowDialog() == true) LoadData();
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b || b.Tag is not MachineType mt) return;

        using var db = new AppDbContext();
        bool inUse = db.CircuitElements.Any(x => x.MachineTypeId == mt.Id);
        if (inUse)
        {
            MessageBox.Show("Nie można usunąć: typ jest używany przez CircuitElements.",
                "Blocked", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var tracked = db.MachineTypes.First(x => x.Id == mt.Id);
        db.MachineTypes.Remove(tracked);
        db.SaveChanges();
        LoadData();
    }
}
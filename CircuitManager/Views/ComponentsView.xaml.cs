using System.Windows;
using System.Windows.Controls;
using CircuitManager.Data;
using CircuitManager.Models;

namespace CircuitManager.Views;

public partial class ComponentsView : UserControl
{
    public ComponentsView()
    {
        InitializeComponent();
        LoadComponents();
    }

    private void LoadComponents()
    {
        using var db = new AppDbContext();
        ComponentsGrid.ItemsSource = db.Components
            .OrderBy(c => c.Label)
            .ThenBy(c => c.Name)
            .ToList();
    }

    private void AddComponent_Click(object sender, RoutedEventArgs e)
    {
        var dlg = new EditComponentWindow(); // tryb ADD
        if (dlg.ShowDialog() == true)
        {
            using var db = new AppDbContext();
            db.Components.Add(dlg.ResultComponent);
            db.SaveChanges();
            LoadComponents();
        }
    }

    private void Edit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b || b.Tag is not Component comp) return;

        var dlg = new EditComponentWindow(comp.Id); // tryb EDIT
        if (dlg.ShowDialog() == true)
        {
            using var db = new AppDbContext();
            // zaktualizuj z DB
            var tracked = db.Components.First(x => x.Id == comp.Id);
            tracked.Label = dlg.ResultComponent.Label;
            tracked.Name  = dlg.ResultComponent.Name;
            tracked.Direction = dlg.ResultComponent.Direction;
            db.SaveChanges();
            LoadComponents();
        }
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b || b.Tag is not Component comp) return;

        using var db = new AppDbContext();

        // sprawdź, czy komponent jest używany w jakimkolwiek CircuitElement
        bool inUse = db.CircuitElements.Any(el => el.ComponentList.Any(c => c.Id == comp.Id));
        if (inUse)
        {
            MessageBox.Show("Nie można usunąć: komponent jest używany przez elementy układu.",
                "Blocked", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var tracked = db.Components.First(x => x.Id == comp.Id);
        db.Components.Remove(tracked);
        db.SaveChanges();
        LoadComponents();
    }
}
using System.Windows;
using System.Windows.Controls;
using CircuitManager.Data;

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
        ComponentsGrid.ItemsSource = db.Components.ToList();
    }


    private void AddComponent_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddComponentWindow();
        if (dialog.ShowDialog() == true)
        {
            using var db = new AppDbContext();
            db.Components.Add(dialog.NewComponent);
            db.SaveChanges();
            LoadComponents();
        }
    }
}
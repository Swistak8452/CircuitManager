using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CircuitManager.Data;
using CircuitManager.Models;

namespace CircuitManager.Views;
public partial class CircuitElementsView : UserControl
{
    public CircuitElementsView()
    {
        InitializeComponent();
        LoadElements();
    }


    private void LoadElements()
    {
        using var db = new AppDbContext();
        ElementsGrid.ItemsSource = db.CircuitElements.ToList();
    }


    private void AddElement_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new EditCircuitElementWindow();
        if (dialog.ShowDialog() == true)
        {
            LoadElements();
        }
    }


    private void EditElement_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is CircuitElement element)
        {
            var dialog = new EditCircuitElementWindow(element.Id);
            if (dialog.ShowDialog() == true)
            {
                LoadElements();
            }
        }
    }
}
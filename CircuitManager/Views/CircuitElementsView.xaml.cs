using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CircuitManager.Data;
using CircuitManager.Models;
using Microsoft.EntityFrameworkCore;

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
        ElementsGrid.ItemsSource = db.CircuitElements.Include(e => e.MachineType).ToList();
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
    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button b || b.Tag is not CircuitElement el) return;

        using var db = new AppDbContext();

        // nie pozwól usunąć, jeśli ktoś wskazuje na ten element jako Next
        bool referencedAsNext = db.CircuitElements.Any(x => x.NextCircuitElementId == el.Id);
        if (referencedAsNext)
        {
            MessageBox.Show("Nie można usunąć: element jest ustawiony jako Next w innych elementach.",
                "Blocked", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // wyczyść powiązania many-to-many (żeby nie zostawały osierocone wpisy)
        var tracked = db.CircuitElements
            .Include(x => x.ComponentList)
            .First(x => x.Id == el.Id);

        tracked.ComponentList.Clear();
        db.CircuitElements.Remove(tracked);
        db.SaveChanges();
        LoadElements();
    }

    
}
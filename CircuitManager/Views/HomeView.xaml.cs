using System.Windows;
using System.Windows.Controls;

namespace CircuitManager.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void GenerateJson_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ExportCircuitDialog();
        dialog.ShowDialog();
    }
}
using System.Windows;

namespace CircuitManager.Views;

public partial class JsonViewerDialog : Window
{
    public JsonViewerDialog(string json)
    {
        InitializeComponent();
        JsonTextBox.Text = json;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
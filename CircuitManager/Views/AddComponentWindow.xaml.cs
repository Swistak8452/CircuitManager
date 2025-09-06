namespace CircuitManager.Views;

using System.Windows;
using Models;

public partial class AddComponentWindow : Window
{
    public Component NewComponent { get; private set; } = new Component();


    public AddComponentWindow()
    {
        InitializeComponent();
    }


    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }


    private void Save_Click(object sender, RoutedEventArgs e)
    {
        NewComponent.Shortcut = ShortcutBox.Text.Trim();
        NewComponent.Name = NameBox.Text.Trim();


        if (string.IsNullOrWhiteSpace(NewComponent.Shortcut) || string.IsNullOrWhiteSpace(NewComponent.Name))
        {
            MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }


        DialogResult = true;
        Close();
    }
}
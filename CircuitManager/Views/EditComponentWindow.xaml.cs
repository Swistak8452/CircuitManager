using CircuitManager.Data;

namespace CircuitManager.Views;

using System.Windows;
using Models;

public partial class EditComponentWindow : Window
{
    private readonly int? _id;
    public Component ResultComponent { get; private set; } = new();

    public EditComponentWindow(int? id = null)
    {
        InitializeComponent();
        _id = id;
        LoadData();
    }

    private void LoadData()
    {
        if (_id is null)
        {
            Title = "Add Component";
            DirectionCombo.SelectedIndex = 0; // domyślnie Input
            return;
        }

        Title = "Edit Component";

        using var db = new AppDbContext();
        var comp = db.Components.First(x => x.Id == _id);

        // wypełnij pola
        LabelBox.Text = comp.Label;
        NameBox.Text  = comp.Name;
        DirectionCombo.SelectedIndex = comp.Direction == IODirection.Input ? 0 : 1;

        // wynikowy komponent (użyjemy jego wartości przy zapisie)
        ResultComponent = new Component
        {
            Id = comp.Id,
            Label = comp.Label,
            Name = comp.Name,
            Direction = comp.Direction
        };
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var label = LabelBox.Text.Trim();
        var name  = NameBox.Text.Trim();
        var dir   = DirectionCombo.SelectedIndex == 0 ? IODirection.Input : IODirection.Output;

        if (string.IsNullOrWhiteSpace(label) || string.IsNullOrWhiteSpace(name))
        {
            MessageBox.Show("Label i Name są wymagane.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Walidacja unikalności
        using var db = new AppDbContext();
        bool labelTaken = db.Components.Any(c => c.Label == label && c.Id != (_id ?? 0));
        bool nameTaken  = db.Components.Any(c => c.Name  == name  && c.Id != (_id ?? 0));

        if (labelTaken || nameTaken)
        {
            MessageBox.Show("Label i Name muszą być unikalne.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        ResultComponent.Label = label;
        ResultComponent.Name  = name;
        ResultComponent.Direction = dir;

        DialogResult = true;
        Close();
    }

}
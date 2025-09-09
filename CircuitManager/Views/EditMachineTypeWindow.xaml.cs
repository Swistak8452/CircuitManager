using System.Linq;
using System.Windows;
using CircuitManager.Data;
using CircuitManager.Models;

namespace CircuitManager.Views;

public partial class EditMachineTypeWindow: Window
{
    private readonly int? _id;
        private MachineType _mt = new();

        public EditMachineTypeWindow(int? id = null)
        {
            InitializeComponent();
            _id = id;
            LoadData();
        }

        private void LoadData()
        {
            if (_id is null) return;
            using var db = new AppDbContext();
            _mt = db.MachineTypes.First(x => x.Id == _id);
            LabelBox.Text = _mt.Label;
            NameBox.Text  = _mt.Name;
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

            if (string.IsNullOrWhiteSpace(label) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Both Label and Name are required.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();

            bool labelTaken = db.MachineTypes.Any(t => t.Label == label && t.Id != (_id ?? 0));
            bool nameTaken  = db.MachineTypes.Any(t => t.Name  == name  && t.Id != (_id ?? 0));
            if (labelTaken || nameTaken)
            {
                MessageBox.Show("Label/Name must be unique.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_id is null)
            {
                _mt = new MachineType { Label = label, Name = name };
                db.MachineTypes.Add(_mt);
            }
            else
            {
                _mt = db.MachineTypes.First(x => x.Id == _id);
                _mt.Label = label;
                _mt.Name  = name;
                db.MachineTypes.Update(_mt);
            }

            db.SaveChanges();
            DialogResult = true;
            Close();
        }
}
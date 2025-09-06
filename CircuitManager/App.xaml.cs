using System.Configuration;
using System.Data;
using System.Windows;
using CircuitManager.Data;
using CircuitManager.Data.Seed;

namespace CircuitManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        using (var db = new AppDbContext())
        {
            await AppDbInitializer.EnsureSeededAsync(db);
        }

        new MainWindow().Show();
    }
}
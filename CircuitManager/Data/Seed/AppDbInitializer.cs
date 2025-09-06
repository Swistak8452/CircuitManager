using CircuitManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CircuitManager.Data.Seed;

public static class AppDbInitializer
{
    public static async Task EnsureSeededAsync(AppDbContext db)
    {
        // 1) Tworzenie/aktualizacja schematu
        await db.Database.MigrateAsync();

        var presets = new[]
        {
            new Component { Shortcut = "N", Name = "Napęd" },
            new Component { Shortcut = "P", Name = "Przycisk" },
            new Component { Shortcut = "S", Name = "Stycznik" },
            new Component { Shortcut = "C", Name = "Czujnik" },
            new Component { Shortcut = "R", Name = "Przenośnik" },
            new Component { Shortcut = "K", Name = "Korek" },
        };

        foreach (var p in presets)
        {
            // sprawdzaj po Shortcut
            bool exists = await db.Components.AnyAsync(x => x.Shortcut == p.Shortcut);
            if (!exists)
                db.Components.Add(p);
        }

        await db.SaveChangesAsync();

    }
}
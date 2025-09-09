using CircuitManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CircuitManager.Data.Seed;

public static class AppDbInitializer
{
    public static async Task EnsureSeededAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

    }
}
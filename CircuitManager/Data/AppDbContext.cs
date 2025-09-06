namespace CircuitManager.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Models;

public class AppDbContext : DbContext
{
    public DbSet<Component> Components { get; set; }
    public DbSet<CircuitElement> CircuitElements { get; set; }

    public string DbPath { get; }

    public AppDbContext()
    {
        // Ścieżka do pliku .db (np. w AppData)
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CircuitManager");
        Directory.CreateDirectory(dir);
        DbPath = Path.Combine(dir, "app.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unikalność dla Component.Name
        modelBuilder.Entity<Component>()
            .HasIndex(c => c.Name)
            .IsUnique();
        
        // Unikalność dla CircuitElement.Shortcut
        modelBuilder.Entity<Component>()
            .HasIndex(c => c.Shortcut)
            .IsUnique();

        // Unikalność dla CircuitElement.Name
        modelBuilder.Entity<CircuitElement>()
            .HasIndex(e => e.Name)
            .IsUnique();
        
        // relacja: CircuitElement → NextCircuitElement (self-reference)
        modelBuilder.Entity<CircuitElement>()
            .HasOne(e => e.NextCircuitElement)
            .WithMany()
            .HasForeignKey(e => e.NextCircuitElementId)
            .OnDelete(DeleteBehavior.Restrict);

        // many-to-many: CircuitElement ↔ Component
        modelBuilder.Entity<CircuitElement>()
            .HasMany(e => e.ComponentList)
            .WithMany(c => c.CircuitElements)
            .UsingEntity(j => j.ToTable("CircuitElementComponents"));
    }
}
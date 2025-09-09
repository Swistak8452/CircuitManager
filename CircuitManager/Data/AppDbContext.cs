namespace CircuitManager.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using CircuitManager.Models;

public class AppDbContext : DbContext
{
    public DbSet<Component> Components { get; set; }
    public DbSet<CircuitElement> CircuitElements { get; set; }
    public DbSet<MachineType> MachineTypes { get; set; } = null!;

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
        
        modelBuilder.Entity<Component>()
            .HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Component>()
            .HasIndex(c => c.Label).IsUnique();

        modelBuilder.Entity<MachineType>()
            .HasIndex(t => t.Label).IsUnique();
        modelBuilder.Entity<MachineType>()
            .HasIndex(t => t.Name).IsUnique();

        modelBuilder.Entity<CircuitElement>()
            .HasIndex(e => e.Name).IsUnique();

        modelBuilder.Entity<CircuitElement>()
            .HasOne(e => e.NextCircuitElement)
            .WithMany()
            .HasForeignKey(e => e.NextCircuitElementId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CircuitElement>()
            .HasOne(e => e.MachineType)
            .WithMany()
            .HasForeignKey(e => e.MachineTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<CircuitElement>()
            .HasMany(e => e.ComponentList)
            .WithMany(c => c.CircuitElements)
            .UsingEntity(j => j.ToTable("CircuitElementComponents"));
        
        modelBuilder.Entity<MachineType>().HasData(
            new MachineType { Id = 1, Label = "TP", Name = "Transporter Palet" },
            new MachineType { Id = 2, Label = "TO", Name = "Obrotnica" },
            new MachineType { Id = 3, Label = "MP", Name = "Magazyn Palet" }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Id = 1, Label = "N", Name = "Napęd",      Direction = IODirection.Output },
            new Component { Id = 2, Label = "P", Name = "Przycisk",   Direction = IODirection.Input  },
            new Component { Id = 3, Label = "S", Name = "Stycznik",   Direction = IODirection.Output },
            new Component { Id = 4, Label = "C", Name = "Czujnik",    Direction = IODirection.Input  },
            new Component { Id = 5, Label = "R", Name = "Przenośnik", Direction = IODirection.Output },
            new Component { Id = 6, Label = "K", Name = "Korek",      Direction = IODirection.Input  }
        );
        
    }
}
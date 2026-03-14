using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace AuditManager.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Auditoria> Auditorias { get; set; } = null!;
    public DbSet<Hallazgo> Hallazgos { get; set; } = null!;
    public DbSet<Responsable> Responsables { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Guardar enums como enteros (INT) en SQL Server, no como texto
        modelBuilder.Entity<Auditoria>()
            .Property(a => a.Estado)
            .HasConversion<int>();

        modelBuilder.Entity<Hallazgo>()
            .Property(h => h.Tipo)
            .HasConversion<int>();

        modelBuilder.Entity<Hallazgo>()
            .Property(h => h.Severidad)
            .HasConversion<int>();
    }
}

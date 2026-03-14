using AuditManager.Core.Entities;
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

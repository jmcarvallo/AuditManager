using AuditManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditManager.Infrastructure.Data.Configurations;

public class HallazgoConfiguration : IEntityTypeConfiguration<Hallazgo>
{
    public void Configure(EntityTypeBuilder<Hallazgo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Descripcion)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Tipo)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Severidad)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(x => x.Auditoria)
            .WithMany(a => a.Hallazgos)
            .HasForeignKey(x => x.AuditoriaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

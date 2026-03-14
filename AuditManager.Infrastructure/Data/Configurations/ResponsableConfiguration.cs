using AuditManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditManager.Infrastructure.Data.Configurations;

public class ResponsableConfiguration : IEntityTypeConfiguration<Responsable>
{
    public void Configure(EntityTypeBuilder<Responsable> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Correo)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(x => x.Correo).IsUnique();

        builder.Property(x => x.Area)
            .IsRequired()
            .HasMaxLength(150);
            
        builder.Navigation(x => x.Auditorias).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

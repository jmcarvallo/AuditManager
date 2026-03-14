using AuditManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuditManager.Infrastructure.Data.Configurations;

public class AuditoriaConfiguration : IEntityTypeConfiguration<Auditoria>
{
    public void Configure(EntityTypeBuilder<Auditoria> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.AreaAuditada)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Estado)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasOne(x => x.Responsable)
            .WithMany(r => r.Auditorias)
            .HasForeignKey(x => x.ResponsableId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Hallazgos).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

using System;
using AuditManager.Core.Enums;

namespace AuditManager.Core.Entities;

public class Hallazgo
{
    public required Guid Id { get; init; }
    public required Guid AuditoriaId { get; set; }
    public required string Descripcion { get; set; }
    public required TipoHallazgo Tipo { get; set; }
    public required SeveridadHallazgo Severidad { get; set; }
    public required DateTime FechaDeteccion { get; set; }

    // Navigation property
    public Auditoria? Auditoria { get; set; }
}

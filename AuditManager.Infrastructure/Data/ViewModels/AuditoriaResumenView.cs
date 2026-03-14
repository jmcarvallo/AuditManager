using System;

namespace AuditManager.Infrastructure.Data.ViewModels;

/// <summary>
/// Modelo de lectura que mapea directamente la vista SQL vw_AuditoriasFinalizadasResumen.
/// Usado como entidad keyless (HasNoKey) en EF Core.
/// </summary>
public class AuditoriaResumenView
{
    public Guid AuditoriaId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string AreaAuditada { get; set; } = string.Empty;
    public string Responsable { get; set; } = string.Empty;
    public int HallazgosBaja { get; set; }
    public int HallazgosMedia { get; set; }
    public int HallazgosAlta { get; set; }
    public int TotalHallazgos { get; set; }
}

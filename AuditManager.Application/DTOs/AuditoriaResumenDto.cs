using System;

namespace AuditManager.Application.DTOs;

public record AuditoriaResumenDto(
    Guid AuditoriaId,
    string Titulo,
    DateTime FechaInicio,
    DateTime FechaFin,
    string AreaAuditada,
    string Responsable,
    int HallazgosBaja,
    int HallazgosMedia,
    int HallazgosAlta,
    int TotalHallazgos
);

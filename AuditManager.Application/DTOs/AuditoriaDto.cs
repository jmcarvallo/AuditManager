using System;
using AuditManager.Core.Enums;

namespace AuditManager.Application.DTOs;

public record AuditoriaDto(
    Guid Id,
    string Titulo,
    DateTime FechaInicio,
    DateTime FechaFin,
    string AreaAuditada,
    Guid ResponsableId,
    EstadoAuditoria Estado
);

using System;
using AuditManager.Core.Enums;

namespace AuditManager.Application.DTOs;

public record HallazgoDto(
    Guid Id,
    Guid AuditoriaId,
    string Descripcion,
    TipoHallazgo Tipo,
    SeveridadHallazgo Severidad,
    DateTime FechaDeteccion
);

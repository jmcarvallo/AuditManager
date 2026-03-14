using System;
using AuditManager.Core.Enums;
using MediatR;

namespace AuditManager.Application.Features.Hallazgos.Commands.Create;

public record CreateHallazgoCommand(
    Guid AuditoriaId,
    string Descripcion,
    TipoHallazgo Tipo,
    SeveridadHallazgo Severidad,
    DateTime FechaDeteccion
) : IRequest<Guid>;

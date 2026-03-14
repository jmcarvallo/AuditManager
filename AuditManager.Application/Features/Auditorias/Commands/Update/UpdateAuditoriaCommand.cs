using System;
using MediatR;
using AuditManager.Application.DTOs;

namespace AuditManager.Application.Features.Auditorias.Commands.Update;

public record UpdateAuditoriaCommand(
    Guid Id,
    string Titulo,
    DateTime FechaInicio,
    DateTime FechaFin,
    string AreaAuditada,
    Guid ResponsableId
) : IRequest<AuditoriaDto>;

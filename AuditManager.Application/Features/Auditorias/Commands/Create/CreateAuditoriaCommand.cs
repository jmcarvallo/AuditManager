using System;
using AuditManager.Application.DTOs;
using MediatR;

namespace AuditManager.Application.Features.Auditorias.Commands.Create;

public record CreateAuditoriaCommand(
    string Titulo,
    DateTime FechaInicio,
    DateTime FechaFin,
    string AreaAuditada,
    Guid ResponsableId
) : IRequest<AuditoriaDto>;

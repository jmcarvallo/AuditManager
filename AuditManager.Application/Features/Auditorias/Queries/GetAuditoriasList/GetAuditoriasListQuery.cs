using System;
using System.Collections.Generic;
using AuditManager.Application.DTOs;
using AuditManager.Core.Enums;
using MediatR;

namespace AuditManager.Application.Features.Auditorias.Queries.GetAuditoriasList;

public record GetAuditoriasListQuery(
    DateTime? FechaInicio, 
    DateTime? FechaFin, 
    EstadoAuditoria? Estado
) : IRequest<List<AuditoriaDto>>;

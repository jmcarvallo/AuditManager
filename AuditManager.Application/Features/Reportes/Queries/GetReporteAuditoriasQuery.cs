using System;
using System.Collections.Generic;
using AuditManager.Application.DTOs;
using MediatR;

namespace AuditManager.Application.Features.Reportes.Queries;

public record GetReporteAuditoriasQuery(
    DateTime? FechaInicio = null,
    DateTime? FechaFin = null
) : IRequest<List<AuditoriaResumenDto>>;

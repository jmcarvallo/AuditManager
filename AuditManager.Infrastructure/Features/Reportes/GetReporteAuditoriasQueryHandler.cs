using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Reportes.Queries;
using AuditManager.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuditManager.Infrastructure.Features.Reportes;

public class GetReporteAuditoriasQueryHandler(AppDbContext dbContext)
    : IRequestHandler<GetReporteAuditoriasQuery, List<AuditoriaResumenDto>>
{
    public async Task<List<AuditoriaResumenDto>> Handle(GetReporteAuditoriasQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.AuditoriasResumen.AsQueryable();

        if (request.FechaInicio.HasValue)
            query = query.Where(v => v.FechaFin >= request.FechaInicio.Value);

        if (request.FechaFin.HasValue)
            query = query.Where(v => v.FechaFin <= request.FechaFin.Value);

        var results = await query
            .OrderByDescending(v => v.FechaFin)
            .ToListAsync(cancellationToken);

        return results.Select(v => new AuditoriaResumenDto(
            v.AuditoriaId,
            v.Titulo,
            v.FechaInicio,
            v.FechaFin,
            v.AreaAuditada,
            v.Responsable,
            v.HallazgosBaja,
            v.HallazgosMedia,
            v.HallazgosAlta,
            v.TotalHallazgos
        )).ToList();
    }
}

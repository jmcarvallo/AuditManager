using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using MediatR;

namespace AuditManager.Application.Features.Auditorias.Queries.GetAuditoriasList;

public class GetAuditoriasListQueryHandler(IRepository<Auditoria> repository) 
    : IRequestHandler<GetAuditoriasListQuery, List<AuditoriaDto>>
{
    public async Task<List<AuditoriaDto>> Handle(GetAuditoriasListQuery request, CancellationToken cancellationToken)
    {
        var allAudits = await repository.GetAllAsync(cancellationToken);
        
        var query = allAudits.AsQueryable();

        if (request.FechaInicio.HasValue)
            query = query.Where(a => a.FechaInicio >= request.FechaInicio.Value);

        if (request.FechaFin.HasValue)
            query = query.Where(a => a.FechaFin <= request.FechaFin.Value);

        if (request.Estado.HasValue)
            query = query.Where(a => a.Estado == request.Estado.Value);

        if (request.ResponsableId.HasValue)
            query = query.Where(a => a.ResponsableId == request.ResponsableId.Value);

        return query.Select(a => new AuditoriaDto(
            a.Id, 
            a.Titulo, 
            a.FechaInicio, 
            a.FechaFin, 
            a.AreaAuditada, 
            a.ResponsableId, 
            a.Estado)).ToList();
    }
}

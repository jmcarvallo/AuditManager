using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using MediatR;

namespace AuditManager.Application.Features.Hallazgos.Queries;

public class GetHallazgosByAuditoriaQueryHandler(IRepository<Hallazgo> repository)
    : IRequestHandler<GetHallazgosByAuditoriaQuery, List<HallazgoDto>>
{
    public async Task<List<HallazgoDto>> Handle(GetHallazgosByAuditoriaQuery request, CancellationToken cancellationToken)
    {
        var all = await repository.GetAllAsync(cancellationToken);
        return all
            .Where(h => h.AuditoriaId == request.AuditoriaId)
            .Select(h => new HallazgoDto(
                h.Id,
                h.AuditoriaId,
                h.Descripcion,
                h.Tipo,
                h.Severidad,
                h.FechaDeteccion))
            .ToList();
    }
}

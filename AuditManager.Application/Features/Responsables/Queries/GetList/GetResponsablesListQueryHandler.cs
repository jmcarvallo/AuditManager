using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using MediatR;

namespace AuditManager.Application.Features.Responsables.Queries.GetList;

public class GetResponsablesListQueryHandler(IRepository<Responsable> responsableRepository) 
    : IRequestHandler<GetResponsablesListQuery, List<ResponsableDto>>
{
    public async Task<List<ResponsableDto>> Handle(GetResponsablesListQuery request, CancellationToken cancellationToken)
    {
        var responsables = await responsableRepository.GetAllAsync(cancellationToken);
        return responsables.Select(r => new ResponsableDto 
        { 
            Id = r.Id, 
            Nombre = r.Nombre, 
            Correo = r.Correo, 
            Area = r.Area 
        }).ToList();
    }
}

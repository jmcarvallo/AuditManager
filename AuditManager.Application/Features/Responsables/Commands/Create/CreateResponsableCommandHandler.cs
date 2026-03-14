using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using MediatR;

namespace AuditManager.Application.Features.Responsables.Commands.Create;

public class CreateResponsableCommandHandler(IRepository<Responsable> repository) : IRequestHandler<CreateResponsableCommand, Guid>
{
    public async Task<Guid> Handle(CreateResponsableCommand request, CancellationToken cancellationToken)
    {
        var entity = new Responsable
        {
            Id = Guid.NewGuid(),
            Nombre = request.Nombre,
            Correo = request.Correo,
            Area = request.Area
        };

        var created = await repository.AddAsync(entity, cancellationToken);
        return created.Id;
    }
}

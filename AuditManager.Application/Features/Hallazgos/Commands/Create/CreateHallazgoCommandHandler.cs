using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using MediatR;

namespace AuditManager.Application.Features.Hallazgos.Commands.Create;

public class CreateHallazgoCommandHandler(IRepository<Hallazgo> repository, IRepository<Auditoria> auditoriaRepository) 
    : IRequestHandler<CreateHallazgoCommand, Guid>
{
    public async Task<Guid> Handle(CreateHallazgoCommand request, CancellationToken cancellationToken)
    {
        var auditoria = await auditoriaRepository.GetByIdAsync(request.AuditoriaId, cancellationToken);
        if (auditoria == null)
            throw new ArgumentException("La auditoría especificada no existe.");

        var hallazgo = new Hallazgo
        {
            Id = Guid.NewGuid(),
            AuditoriaId = request.AuditoriaId,
            Descripcion = request.Descripcion,
            Tipo = request.Tipo,
            Severidad = request.Severidad,
            FechaDeteccion = request.FechaDeteccion
        };

        var result = await repository.AddAsync(hallazgo, cancellationToken);
        return result.Id;
    }
}

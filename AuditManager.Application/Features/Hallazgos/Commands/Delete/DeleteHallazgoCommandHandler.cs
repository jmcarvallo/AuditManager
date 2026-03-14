using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using MediatR;

namespace AuditManager.Application.Features.Hallazgos.Commands.Delete;

public class DeleteHallazgoCommandHandler(IRepository<Hallazgo> repository, IRepository<Auditoria> auditoriaRepository) 
    : IRequestHandler<DeleteHallazgoCommand, bool>
{
    public async Task<bool> Handle(DeleteHallazgoCommand request, CancellationToken cancellationToken)
    {
        var hallazgo = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (hallazgo == null)
            return false;

        var auditoria = await auditoriaRepository.GetByIdAsync(hallazgo.AuditoriaId, cancellationToken);
        if (auditoria == null)
            throw new InvalidOperationException("La auditoría asociada ya no existe.");

        // Rule: Eliminar hallazgos (solo si la auditoría está “En Proceso”)
        if (auditoria.Estado != EstadoAuditoria.EnProceso)
            throw new InvalidOperationException("Solo se pueden eliminar hallazgos si la auditoría se encuentra 'En Proceso'.");

        await repository.DeleteAsync(hallazgo, cancellationToken);
        return true;
    }
}

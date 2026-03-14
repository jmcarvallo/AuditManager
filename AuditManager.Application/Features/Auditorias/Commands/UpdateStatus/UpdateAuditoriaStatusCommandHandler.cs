using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using MediatR;

namespace AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;

public class UpdateAuditoriaStatusCommandHandler(IRepository<Auditoria> repository) 
    : IRequestHandler<UpdateAuditoriaStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateAuditoriaStatusCommand request, CancellationToken cancellationToken)
    {
        var auditoria = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (auditoria == null)
            return false;

        if (auditoria.Estado != EstadoAuditoria.Pendiente && request.NuevoEstado == EstadoAuditoria.EnProceso)
        {
             throw new InvalidOperationException("Solo se puede pasar a EnProceso si la auditoria está Pendiente.");
        }
        
        if (auditoria.Estado != EstadoAuditoria.EnProceso && request.NuevoEstado == EstadoAuditoria.Finalizada)
        {
             throw new InvalidOperationException("Solo se puede finalizar una auditoria que está EnProceso.");
        }

        auditoria.Estado = request.NuevoEstado;
        await repository.UpdateAsync(auditoria, cancellationToken);
        
        return true;
    }
}

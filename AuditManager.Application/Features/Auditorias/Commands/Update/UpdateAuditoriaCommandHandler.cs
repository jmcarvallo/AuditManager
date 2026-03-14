using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using MediatR;

namespace AuditManager.Application.Features.Auditorias.Commands.Update;

public class UpdateAuditoriaCommandHandler(IRepository<Auditoria> repository, IRepository<Responsable> responsableRepository) 
    : IRequestHandler<UpdateAuditoriaCommand, AuditoriaDto>
{
    public async Task<AuditoriaDto> Handle(UpdateAuditoriaCommand request, CancellationToken cancellationToken)
    {
        var auditoria = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (auditoria == null)
            throw new ArgumentException("Auditoría no encontrada.");

        // Business rule from PDF: Solo se puede actualizar si está en estado "Pendiente"
        if (auditoria.Estado != EstadoAuditoria.Pendiente)
            throw new InvalidOperationException("Solo se puede actualizar una auditoría en estado Pendiente.");

        var resp = await responsableRepository.GetByIdAsync(request.ResponsableId, cancellationToken);
        if (resp == null)
            throw new ArgumentException("El responsable asignado no existe.");

        auditoria.Titulo = request.Titulo;
        auditoria.FechaInicio = request.FechaInicio;
        auditoria.FechaFin = request.FechaFin;
        auditoria.AreaAuditada = request.AreaAuditada;
        auditoria.ResponsableId = request.ResponsableId;

        await repository.UpdateAsync(auditoria, cancellationToken);

        return new AuditoriaDto(
            auditoria.Id, 
            auditoria.Titulo, 
            auditoria.FechaInicio, 
            auditoria.FechaFin, 
            auditoria.AreaAuditada, 
            auditoria.ResponsableId, 
            auditoria.Estado);
    }
}

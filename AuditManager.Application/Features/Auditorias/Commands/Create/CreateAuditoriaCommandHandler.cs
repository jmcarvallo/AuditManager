using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Interfaces;
using AuditManager.Core.Entities;
using AuditManager.Core.Enums;
using MediatR;

namespace AuditManager.Application.Features.Auditorias.Commands.Create;

public class CreateAuditoriaCommandHandler(IRepository<Auditoria> repository, IRepository<Responsable> responsableRepository) 
    : IRequestHandler<CreateAuditoriaCommand, AuditoriaDto>
{
    public async Task<AuditoriaDto> Handle(CreateAuditoriaCommand request, CancellationToken cancellationToken)
    {
        var resp = await responsableRepository.GetByIdAsync(request.ResponsableId, cancellationToken);
        if (resp == null)
            throw new ArgumentException("El responsable asignado no existe.");

        var auditoria = new Auditoria
        {
            Id = Guid.NewGuid(),
            Titulo = request.Titulo,
            FechaInicio = request.FechaInicio,
            FechaFin = request.FechaFin,
            AreaAuditada = request.AreaAuditada,
            ResponsableId = request.ResponsableId,
            Estado = EstadoAuditoria.Pendiente // Default value when created
        };

        var result = await repository.AddAsync(auditoria, cancellationToken);

        return new AuditoriaDto(
            result.Id, 
            result.Titulo, 
            result.FechaInicio, 
            result.FechaFin, 
            result.AreaAuditada, 
            result.ResponsableId, 
            result.Estado);
    }
}

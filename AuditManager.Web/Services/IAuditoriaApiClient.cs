using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auditorias.Commands.Create;
using AuditManager.Application.Features.Auditorias.Commands.Update;
using AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;

namespace AuditManager.Web.Services;

public interface IAuditoriaApiClient
{
    Task<List<AuditoriaDto>> GetAuditoriasAsync();
    Task<AuditoriaDto?> GetAuditoriaByIdAsync(Guid id);
    Task<AuditoriaDto?> CreateAuditoriaAsync(CreateAuditoriaCommand command);
    Task<bool> UpdateAuditoriaAsync(Guid id, UpdateAuditoriaCommand command);
    Task<bool> ChangeStatusAsync(Guid id, UpdateAuditoriaStatusCommand command);
}

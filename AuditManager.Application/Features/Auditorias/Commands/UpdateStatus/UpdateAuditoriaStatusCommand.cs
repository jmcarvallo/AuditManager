using System;
using MediatR;
using AuditManager.Core.Enums;

namespace AuditManager.Application.Features.Auditorias.Commands.UpdateStatus;

public record UpdateAuditoriaStatusCommand(Guid Id, EstadoAuditoria NuevoEstado) : IRequest<bool>;

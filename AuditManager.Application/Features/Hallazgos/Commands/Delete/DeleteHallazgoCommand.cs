using System;
using MediatR;

namespace AuditManager.Application.Features.Hallazgos.Commands.Delete;

public record DeleteHallazgoCommand(Guid Id) : IRequest<bool>;

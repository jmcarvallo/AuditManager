using System;
using MediatR;

namespace AuditManager.Application.Features.Responsables.Commands.Create;

public record CreateResponsableCommand(string Nombre, string Correo, string Area) : IRequest<Guid>;

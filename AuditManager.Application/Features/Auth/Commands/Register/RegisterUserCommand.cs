using MediatR;

namespace AuditManager.Application.Features.Auth.Commands.Register;

public record RegisterUserCommand(
    string Username,
    string Correo,
    string Password,
    string Cargo
) : IRequest<Guid>;

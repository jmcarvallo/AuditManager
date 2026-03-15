using AuditManager.Application.DTOs;
using MediatR;

namespace AuditManager.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;

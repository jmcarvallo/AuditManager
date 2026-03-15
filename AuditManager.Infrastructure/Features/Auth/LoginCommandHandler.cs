using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.DTOs;
using AuditManager.Application.Features.Auth.Commands.Login;
using AuditManager.Application.Interfaces;
using AuditManager.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuditManager.Infrastructure.Features.Auth;

public class LoginCommandHandler(AppDbContext db, IJwtTokenGenerator jwtGenerator)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await db.Usuarios
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken)
            ?? throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var hasher = new PasswordHasher<object>();
        var result = hasher.VerifyHashedPassword(new object(), usuario.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var token = jwtGenerator.GenerateToken(usuario);
        return new LoginResponseDto(token, usuario.Username, usuario.Cargo);
    }
}

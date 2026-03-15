using System;
using System.Threading;
using System.Threading.Tasks;
using AuditManager.Application.Features.Auth.Commands.Register;
using AuditManager.Core.Entities;
using AuditManager.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuditManager.Infrastructure.Features.Auth;

public class RegisterUserCommandHandler(AppDbContext db)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existe = await db.Usuarios
            .AnyAsync(u => u.Username == request.Username || u.Correo == request.Correo, cancellationToken);

        if (existe)
            throw new InvalidOperationException("El nombre de usuario o correo ya está registrado.");

        var hasher = new PasswordHasher<object>();
        var hash = hasher.HashPassword(new object(), request.Password);

        var usuario = new Usuario
        {
            Username     = request.Username,
            Correo       = request.Correo,
            PasswordHash = hash,
            Cargo        = request.Cargo,
            CreadoEn     = DateTime.UtcNow
        };

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync(cancellationToken);
        return usuario.Id;
    }
}

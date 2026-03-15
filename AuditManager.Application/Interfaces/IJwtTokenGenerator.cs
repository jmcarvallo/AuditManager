using AuditManager.Core.Entities;

namespace AuditManager.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Usuario usuario);
}

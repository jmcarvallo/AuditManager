using AuditManager.Application.Interfaces;
using AuditManager.Infrastructure.Data;
using AuditManager.Infrastructure.Features.Auth;
using AuditManager.Infrastructure.Features.Reportes;
using AuditManager.Infrastructure.Repositories;
using AuditManager.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditManager.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

        // JWT token generator
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // Registrar handlers de MediatR ubicados en Infrastructure
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

        return services;
    }
}

using AuditManager.Application.Interfaces;
using AuditManager.Infrastructure.Data;
using AuditManager.Infrastructure.Features.Reportes;
using AuditManager.Infrastructure.Repositories;
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

        // Registrar handlers de MediatR ubicados en Infrastructure (ej: reportes con EF directo)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetReporteAuditoriasQueryHandler).Assembly));

        return services;
    }
}

using Application.Abstractions.Authentication;
using Application.Abstractions.Email;
using Domain.IRepositories.UnitOfWorks;
using infrastructure.Authentication.Services;
using infrastructure.Persistence.Interceptors;
using infrastructure.Persistence.Repositories.UnitOfWorks;
using infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IJwtProviderService, JwtProviderService>();

        return services;
    }
}
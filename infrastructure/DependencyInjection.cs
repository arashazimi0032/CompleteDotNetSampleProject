using Application.Abstractions.Email;
using Domain.IRepositories.UnitOfWorks;
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

        return services;
    }
}
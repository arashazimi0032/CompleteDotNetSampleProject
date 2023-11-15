using Application.Abstractions.Email;
using Domain.IRepositories;
using infrastructure.Persistence.Repositories;
using infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}
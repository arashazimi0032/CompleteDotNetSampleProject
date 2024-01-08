using Application.Abstractions.Authentication;
using Application.Abstractions.Email;
using Domain.IRepositories.UnitOfWorks;
using infrastructure.Authentication.Services;
using infrastructure.BackgroundJobs.JobSetup;
using infrastructure.Persistence.Interceptors;
using infrastructure.Persistence.Repositories.UnitOfWorks;
using infrastructure.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

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
        services.AddJobs();

        return services;
    }

    public static void AddJobs(this IServiceCollection services)
    {
        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<LoggingBackgroundJobSetup>();
    }
}
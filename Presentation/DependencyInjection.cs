using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Presentation.OptionsSetup;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection service)
    {
        service.AddCarter();

        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        service.ConfigureOptions<JwtOptionsSetup>();
        service.ConfigureOptions<JwtBearerOptionsSetup>();

        return service;
    }
}
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Presentation.OptionsSetup;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection service)
    {
        service.AddCarter();

        service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        service.AddAuthorization();

        service.ConfigureOptions<JwtOptionsSetup>();
        service.ConfigureOptions<JwtBearerOptionsSetup>();

        return service;
    }
}
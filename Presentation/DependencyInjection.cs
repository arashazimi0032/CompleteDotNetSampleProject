using Carter;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Presentation.OptionsSetup;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddStackExchangeRedisCache(redisOption =>
        {
            var connection = configuration.GetConnectionString("Redis");
            redisOption.Configuration = connection;
        });

        service.AddCarter();

        service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        service.ConfigureOptions<JwtOptionsSetup>();
        service.ConfigureOptions<JwtBearerOptionsSetup>();
        service.ConfigureOptions<EtherealEmailOptionsSetup>();

        service
            .AddAuthorization()
            .AddAuthorizationPolicies();

        return service;
    }

    public static IServiceCollection AddSwaggerGenWhitAuthorize(this IServiceCollection service)
    {
        service.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CompleteDotNetSampleProject", Version = "v1" });

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter Bearer token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            };
            c.AddSecurityRequirement(securityRequirement);
        });

        return service;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection service)
    {
        service.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy =>
            {
                policy
                    .RequireRole(Role.Admin.ToString());
            });

        return service;
    }
}
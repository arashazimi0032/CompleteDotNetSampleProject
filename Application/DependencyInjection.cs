using Application.Behaviors;
using Application.Products.Commands.Create;
using Application.Products.Commands.Delete;
using Application.Products.Commands.Update;
using Application.Products.Queries.Get;
using Application.Products.Queries.GetAll;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
        });

        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<ICreateProductService, CreateProductService>();
        
        services.AddScoped<IDeleteProductService, DeleteProductService>();
        
        services.AddScoped<IUpdateProductService, UpdateProductService>();

        services.AddScoped<IGetProductService, GetProductService>();
        
        services.AddScoped<IGetAllProductsService, GetAllProductsService>();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        
        return services;
    }
}
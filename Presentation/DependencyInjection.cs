using Carter;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection service)
    {
        service.AddCarter();

        return service;
    }
}
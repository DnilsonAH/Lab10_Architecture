using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectureLAB10.Application.Configuration;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // 1. Registrar AutoMapper
        // Escanea el ensamblado actual (Application) en busca de perfiles (MappingProfile)
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // 2. Registrar MediatR
        // Escanea el ensamblado actual en busca de Handlers (RegisterUserCommandHandler)
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        return services;
    }
}
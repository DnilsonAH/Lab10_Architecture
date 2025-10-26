using System.Text;
using ArchitectureLAB10.Domain.Ports;
using ArchitectureLAB10.Domain.Ports.IServices;
using ArchitectureLAB10.Infrastructure.Adapters;
using ArchitectureLAB10.Infrastructure.Adapters.Repositories;
using ArchitectureLAB10.Infrastructure.Adapters.Services;
using ArchitectureLAB10.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ArchitectureLAB10.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Configuración del DbContext (MySQL)
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<TicketeraBdContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null))
        );

        // 2. Registro de Unit of Work y Repositorios
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Los repositorios específicos son creados por el UoW,
        // pero si necesitaras inyectar uno directamente, lo registras aquí:
        // services.AddScoped<IUserRepository, UserRepository>();

        // 3. Registro de Servicios (Adaptadores)
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // 4. Configuración de Autenticación JWT
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!))
                };
            });
        
        return services;
    }
}
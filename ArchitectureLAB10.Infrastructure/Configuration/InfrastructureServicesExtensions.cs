using System.Net;

namespace ArchitectureLAB10.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add infrastructure services here (e.g., database context, repositories, etc.)

        //DataBase Mysql Conection
        services.AddDbContext<TicketeraDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null));
        });

        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUploadFileToAzureStorageService, UploadFileToAzureStorageService>();
        services.AddScoped<IActivityService, ActivityService>();
        return services;
    }
    
}
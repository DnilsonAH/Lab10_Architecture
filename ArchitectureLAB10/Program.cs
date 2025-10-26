using System.Reflection;
using ArchitectureLAB10.Application.Configuration; // Importar
using ArchitectureLAB10.Infrastructure.Configuration; // Importar
using Microsoft.OpenApi.Models; // Para la configuración de Swagger con JWT

var builder = WebApplication.CreateBuilder(args);

// 1. Registrar servicios de Application (AutoMapper, MediatR)
builder.Services.AddApplicationServices();

// 2. Registrar servicios de Infrastructure (DbContext, UoW, JWT, Repos, Servicios)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 3. Configurar Swagger/OpenAPI para que soporte JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ArchitectureLAB10 API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Autorización JWT (ej: Bearer eyJhbGciOi...)"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});


// Importante: Agregar servicios de controladores
builder.Services.AddControllers();

var app = builder.Build();

// Configurar el pipeline de HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Genera el JSON
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArchitectureLAB10 API v1");
    });
}

// 4. Habilitar autenticación y autorización
app.UseAuthentication(); // Primero autentica (verifica quién es)
app.UseAuthorization(); // Luego autoriza (verifica qué puede hacer)

// 5. Mapear controladores
app.MapControllers();

app.Run();
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports.IServices;
using Microsoft.Extensions.Configuration; // Necesario para leer appsettings
using Microsoft.IdentityModel.Tokens;

namespace ArchitectureLAB10.Infrastructure.Adapters.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        //configuración de appsettings
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Creamos los claims (información del usuario en el token)
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            // Aquí roles tambein
        };

        // Creamos el token
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Duración del token
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using ArchitectureLAB10.Domain.Ports.IServices;
using BCrypt.Net; // Importa BCrypt

namespace ArchitectureLAB10.Infrastructure.Adapters.Services;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // HashPassword de BCrypt genera un salt de forma auomática
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
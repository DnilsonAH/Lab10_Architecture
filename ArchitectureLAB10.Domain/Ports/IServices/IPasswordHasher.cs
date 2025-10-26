namespace ArchitectureLAB10.Domain.Ports.IServices;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
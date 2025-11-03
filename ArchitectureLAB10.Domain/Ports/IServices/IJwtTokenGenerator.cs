using ArchitectureLAB10.Domain.Entities;

namespace ArchitectureLAB10.Domain.Ports.IServices;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IEnumerable<string> roles);
}
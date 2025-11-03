using ArchitectureLAB10.Domain.Entities;

namespace ArchitectureLAB10.Domain.Ports.IRepositories;

public interface IUserRoleRepository : IGenericRepository<UserRole>
{
    Task<IEnumerable<UserRole>> GetRolesByUserIdAsync(Guid userId);
}
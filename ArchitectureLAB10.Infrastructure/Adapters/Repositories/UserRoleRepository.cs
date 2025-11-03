using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureLAB10.Infrastructure.Adapters.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(TicketeraBdContext context) : base(context)
    {
    }
    public async Task<IEnumerable<UserRole>> GetRolesByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(ur => ur.Role) //obtener rol del usuario
            .Where(ur => ur.UserId == userId)
            .ToListAsync();
    }
}
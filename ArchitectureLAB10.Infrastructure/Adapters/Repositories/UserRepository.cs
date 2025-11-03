using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureLAB10.Infrastructure.Adapters.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TicketeraBdContext context) : base(context)
    {
    }
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }
}
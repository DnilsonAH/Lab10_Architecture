using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Data;

namespace ArchitectureLAB10.Infrastructure.Adapters.Repositories;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
    public UserRoleRepository(TicketeraBdContext context) : base(context)
    {
    }
}
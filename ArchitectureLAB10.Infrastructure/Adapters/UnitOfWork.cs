using ArchitectureLAB10.Domain.Ports;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Adapters.Repositories;
using ArchitectureLAB10.Infrastructure.Data;

namespace ArchitectureLAB10.Infrastructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    private readonly TicketeraBdContext _context;
    public IUserRepository UserRepository{ get; }
    public ITicketRepository TicketRepository{ get; }
    public IRoleRepository RoleRepository{ get; }
    public IUserRoleRepository UserRoleRepository{ get; }
    public IResponseRepository ResponseRepository{ get; }

    public UnitOfWork(TicketeraBdContext context,
        IUserRepository userRepository,
        ITicketRepository ticketRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IResponseRepository responseRepository
    )
    {
        _context = context;
        UserRepository = userRepository;
        TicketRepository = ticketRepository;
        RoleRepository = roleRepository;
        UserRoleRepository = userRoleRepository;
        ResponseRepository = responseRepository;
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
using ArchitectureLAB10.Domain.Ports;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Adapters.Repositories;
using ArchitectureLAB10.Infrastructure.Data;

namespace ArchitectureLAB10.Infrastructure.Adapters;

public class UnitOfWork : IUnitOfWork
{
    private readonly TicketeraBdContext _context;
    private IUserRepository? _userRepository;
    private ITicketRepository? _ticketRepository;
    private IRoleRepository? _roleRepository;
    private IUserRoleRepository? _userRoleRepository;
    private IResponseRepository? _responseRepository;

    public UnitOfWork(TicketeraBdContext context)
    {
        _context = context;
    }

    // Inicialización perezosa (Lazy loading) de los repositorios
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public ITicketRepository TicketRepository => _ticketRepository ??= new TicketRepository(_context);
    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);
    public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new UserRoleRepository(_context);
    public IResponseRepository ResponseRepository => _responseRepository ??= new ResponseRepository(_context);


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
using ArchitectureLAB10.Domain.Ports.IRepositories;

namespace ArchitectureLAB10.Domain.Ports;

public interface IUnitOfWork : IDisposable
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    ITicketRepository TicketRepository { get; }
    IResponseRepository ResponseRepository { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
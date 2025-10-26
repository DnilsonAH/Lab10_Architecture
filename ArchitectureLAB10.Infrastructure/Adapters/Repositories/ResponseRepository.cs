using ArchitectureLAB10.Domain.Entities;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Data;

namespace ArchitectureLAB10.Infrastructure.Adapters.Repositories;

public class ResponseRepository : GenericRepository<Response>, IResponseRepository
{
    public ResponseRepository(TicketeraBdContext context) : base(context)
    {
    }
    
}
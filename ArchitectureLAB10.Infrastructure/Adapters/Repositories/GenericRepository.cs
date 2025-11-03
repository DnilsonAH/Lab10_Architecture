using System.Linq.Expressions;
using ArchitectureLAB10.Domain.Ports.IRepositories;
using ArchitectureLAB10.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArchitectureLAB10.Infrastructure.Adapters.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TicketeraBdContext _context;
    protected readonly DbSet<T> _dbSet;
    
    public GenericRepository(TicketeraBdContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }
    // Nueva implementación
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
    
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
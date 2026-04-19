using GymApi.Application.Interfaces;
using GymApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymApi.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly GymDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(GymDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<(List<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbSet.AsQueryable();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
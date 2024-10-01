using Microsoft.EntityFrameworkCore;
using PB303Pronia.Contexts;
using PB303Pronia.Models.Common;
using PB303Pronia.Repositories.Abstractions.Generic;
using System.Linq.Expressions;

namespace PB303Pronia.Repositories.Implementations.Generic;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IQueryable<T> GetAll()
    {
        var query = _context.Set<T>().AsQueryable();

        return query;
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        var entity = await _context.Set<T>().FirstOrDefaultAsync(expression);


        return entity;
    }

    public IQueryable<T> GetFilter(Expression<Func<T, bool>> expression)
    {
        var query = _context.Set<T>().Where(expression);

        return query;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}

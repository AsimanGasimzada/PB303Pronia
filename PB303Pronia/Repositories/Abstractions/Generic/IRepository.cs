using PB303Pronia.Models.Common;
using System.Linq.Expressions;

namespace PB303Pronia.Repositories.Abstractions.Generic;

public interface IRepository<T> where T : BaseEntity, new()
{
    IQueryable<T> GetAll();
    IQueryable<T> GetFilter(Expression<Func<T,bool>> expression);
    Task<T?> GetAsync(Expression<Func<T,bool>> expression);

    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<int> SaveChangesAsync();
}

using System.Linq.Expressions;

namespace MarketPlaceCore.Core.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> FindWithIncludes(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}

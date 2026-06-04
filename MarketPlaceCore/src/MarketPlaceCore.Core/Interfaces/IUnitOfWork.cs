using MarketPlaceCore.Core.Entities;

namespace MarketPlaceCore.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Product> Products { get; }
    IGenericRepository<Category> Categories { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<User> Users { get; }
    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

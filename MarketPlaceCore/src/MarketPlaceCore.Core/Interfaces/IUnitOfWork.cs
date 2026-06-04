using MarketPlaceCore.Core.Entities;

namespace MarketPlaceCore.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Product> Products { get; }
    IGenericRepository<ProductVariant> ProductVariants { get; }
    IGenericRepository<Category> Categories { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<User> Users { get; }
    IGenericRepository<Coupon> Coupons { get; }
    IGenericRepository<Review> Reviews { get; }
    IGenericRepository<WishlistItem> Wishlist { get; }
    IGenericRepository<ProductAlert> ProductAlerts { get; }

    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

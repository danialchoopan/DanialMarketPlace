namespace MarketPlaceCore.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer"; // Customer, Admin
    public string? PhoneNumber { get; set; }
    public string? OtpCode { get; set; }
    public DateTime? OtpExpiry { get; set; }
}

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
}

public class ProductVariant : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Warranty { get; set; } = "گارانتی اصالت و سلامت فیزیکی";
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class Coupon : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int MaxUsageCount { get; set; }
    public int CurrentUsageCount { get; set; }
    public decimal MinimumOrderAmount { get; set; }
}

public class Review : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; } // 1 to 5
    public bool IsVerifiedPurchase { get; set; }
}

public class WishlistItem : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}

public class ProductAlert : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public bool IsNotified { get; set; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount => TotalAmount - DiscountAmount;
    public string? AppliedCouponCode { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int? ProductVariantId { get; set; }
    public ProductVariant? ProductVariant { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

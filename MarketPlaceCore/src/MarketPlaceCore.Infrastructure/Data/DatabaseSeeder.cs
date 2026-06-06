using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;

namespace MarketPlaceCore.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context, IPasswordHasher hasher)
    {
        if (context.Products.Any()) return;

        var categories = new List<Category>
        {
            new Category { Name = "کالای دیجیتال" },
            new Category { Name = "مد و پوشاک" },
            new Category { Name = "خانه و آشپزخانه" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        var product = new Product
        {
            Name = "گوشی موبایل آیفون 15 Pro",
            Description = "تیتانیوم، حافظه 256، دوربین 48 مگاپیکسل فوق حرفه ای",
            CategoryId = categories[0].Id,
            ImageUrl = "/images/iphone.jpg"
        };
        context.Products.Add(product);
        context.SaveChanges();

        var variants = new List<ProductVariant>
        {
            new ProductVariant { ProductId = product.Id, Color = "مشکی", Size = "256GB", Price = 75000000, Stock = 10, Warranty = "۱۸ ماهه شرکتی" },
            new ProductVariant { ProductId = product.Id, Color = "آبی", Size = "256GB", Price = 76000000, Stock = 5, Warranty = "۱۸ ماهه شرکتی" },
            new ProductVariant { ProductId = product.Id, Color = "سفید", Size = "512GB", Price = 88000000, Stock = 2, Warranty = "گارانتی طلایی" }
        };
        context.ProductVariants.AddRange(variants);

        var admin = new User
        {
            Username = "admin",
            PasswordHash = hasher.Hash("admin123"),
            FullName = "مدیر سیستم",
            Role = "Admin",
            PhoneNumber = "09121111111"
        };
        var customer = new User
        {
            Username = "user",
            PasswordHash = hasher.Hash("user123"),
            FullName = "دانیال",
            Role = "Customer",
            PhoneNumber = "09122222222"
        };
        context.Users.AddRange(admin, customer);
        context.SaveChanges();

        var coupon = new Coupon
        {
            Code = "OFF2024",
            DiscountAmount = 50000,
            ExpiryDate = DateTime.UtcNow.AddMonths(1),
            MaxUsageCount = 100,
            MinimumOrderAmount = 200000
        };
        context.Coupons.Add(coupon);

        var review = new Review
        {
            ProductId = product.Id,
            UserId = customer.Id,
            Comment = "کیفیت ساخت فوق‌العاده است. از خرید خود بسیار راضی هستم.",
            Rating = 5,
            IsVerifiedPurchase = true,
            CreatedAt = DateTime.UtcNow.AddDays(-2)
        };
        context.Reviews.Add(review);
        context.SaveChanges();
    }
}

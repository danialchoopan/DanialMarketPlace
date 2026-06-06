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
            new Category { Name = "کالای دیجیتال", ImageUrl = "/images/digital.jpg" },
            new Category { Name = "مد و پوشاک", ImageUrl = "/images/fashion.jpg" },
            new Category { Name = "خانه و آشپزخانه", ImageUrl = "/images/home.jpg" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        var products = new List<Product>
        {
            new Product
            {
                Name = "گوشی موبایل آیفون 15 Pro",
                Description = "تیتانیوم، حافظه 256، دوربین 48 مگاپیکسل فوق حرفه ای",
                CategoryId = categories[0].Id,
                ImageUrl = "/images/iphone.jpg"
            },
            new Product
            {
                Name = "ساعت هوشمند اپل سری 9",
                Description = "ساعت هوشمند با قابلیت‌های ورزشی پیشرفته و پایش سلامتی",
                CategoryId = categories[0].Id,
                ImageUrl = "/images/watch.jpg"
            },
            new Product
            {
                Name = "کفش ورزشی نایکی مدل Air Max",
                Description = "کفش ورزشی راحت و سبک مناسب برای دویدن و استفاده روزمره",
                CategoryId = categories[1].Id,
                ImageUrl = "/images/shoes.jpg"
            },
            new Product
            {
                Name = "هدفون بی سیم سونی مدل WH-1000XM5",
                Description = "هدفون با قابلیت حذف نویز محیطی و کیفیت صدای استثنایی",
                CategoryId = categories[0].Id,
                ImageUrl = "/images/headphone.jpg"
            }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        // Add variants for iPhone
        var iphone = products[0];
        var variants = new List<ProductVariant>
        {
            new ProductVariant { ProductId = iphone.Id, Color = "مشکی", Size = "256GB", Price = 75000000, Stock = 10, Warranty = "۱۸ ماهه شرکتی" },
            new ProductVariant { ProductId = iphone.Id, Color = "آبی", Size = "256GB", Price = 76000000, Stock = 5, Warranty = "۱۸ ماهه شرکتی" },
            new ProductVariant { ProductId = iphone.Id, Color = "سفید", Size = "512GB", Price = 88000000, Stock = 2, Warranty = "گارانتی طلایی" }
        };

        // Add variants for other products
        foreach(var p in products.Skip(1))
        {
            variants.Add(new ProductVariant { ProductId = p.Id, Color = "استاندارد", Size = "یک سایز", Price = 15000000, Stock = 20, Warranty = "ضمانت اصالت" });
        }

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
            ProductId = iphone.Id,
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

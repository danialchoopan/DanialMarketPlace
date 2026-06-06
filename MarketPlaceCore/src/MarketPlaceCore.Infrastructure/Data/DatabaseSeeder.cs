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
            new Category { Name = "کالای دیجیتال", ImageUrl = "https://images.unsplash.com/photo-1519389950473-47ba0277781c?q=80&w=2070" },
            new Category { Name = "مد و پوشاک", ImageUrl = "https://images.unsplash.com/photo-1445205170230-053b83016050?q=80&w=2071" },
            new Category { Name = "خانه و آشپزخانه", ImageUrl = "https://images.unsplash.com/photo-1556911220-e15b29be8c8f?q=80&w=2070" }
        };
        context.Categories.AddRange(categories);
        context.SaveChanges();

        var products = new List<Product>
        {
            new Product
            {
                Name = "گوشی موبایل آیفون 15 Pro",
                Description = "طراحی تیتانیوم گرید هوافضا، تراشه A17 Pro، و پیشرفته‌ترین سیستم دوربین در آیفون. تجربه نهایی قدرت و زیبایی در دستان شما.",
                CategoryId = categories[0].Id,
                ImageUrl = "https://images.unsplash.com/photo-1696446701796-da61225697cc?q=80&w=2070"
            },
            new Product
            {
                Name = "ساعت هوشمند اپل سری 9",
                Description = "ساعت هوشمند با قابلیت‌های ورزشی پیشرفته، پایش سلامتی دقیق و نمایشگر همیشه روشن درخشان. همراه هوشمند شما در تمام لحظات زندگی.",
                CategoryId = categories[0].Id,
                ImageUrl = "https://images.unsplash.com/photo-1434493907317-a46b5bc78344?q=80&w=2070"
            },
            new Product
            {
                Name = "کفش ورزشی نایکی مدل Air Max",
                Description = "کفش ورزشی راحت و سبک با تکنولوژی کپسول هوا برای نهایت ضربه‌گیری. طراحی مدرن و جسورانه برای استفاده روزمره و تمرینات ورزشی.",
                CategoryId = categories[1].Id,
                ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?q=80&w=2070"
            },
            new Product
            {
                Name = "هدفون بی سیم سونی مدل WH-1000XM5",
                Description = "هدفون با پیشرفته‌ترین تکنولوژی حذف نویز فعال در جهان و کیفیت صدای بی‌نظیر High-Res. عمر باتری ۳۰ ساعته برای لذت بردن طولانی مدت.",
                CategoryId = categories[0].Id,
                ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?q=80&w=2070"
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

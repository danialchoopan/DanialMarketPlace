using MarketPlaceCore.Core.Entities;

namespace MarketPlaceCore.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
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

        var products = new List<Product>
        {
            new Product { Name = "گوشی موبایل آیفون 13", Description = "حافظه 128 گیگابایت", Price = 45000000, Stock = 10, CategoryId = categories[0].Id, ImageUrl = "/images/iphone13.jpg" },
            new Product { Name = "لپ‌تاپ ایسوس", Description = "Core i7 16GB RAM", Price = 55000000, Stock = 5, CategoryId = categories[0].Id, ImageUrl = "/images/laptop.jpg" },
            new Product { Name = "تی‌شرت مردانه", Description = "نخی ۱۰۰ درصد", Price = 450000, Stock = 50, CategoryId = categories[1].Id, ImageUrl = "/images/tshirt.jpg" },
            new Product { Name = "کفش ورزشی", Description = "مناسب دویدن", Price = 1200000, Stock = 20, CategoryId = categories[1].Id, ImageUrl = "/images/shoes.jpg" },
            new Product { Name = "قهوه‌ساز", Description = "اتوماتیک", Price = 8500000, Stock = 8, CategoryId = categories[2].Id, ImageUrl = "/images/coffee.jpg" }
        };

        context.Products.AddRange(products);
        context.SaveChanges();

        var admin = new User
        {
            Username = "admin",
            PasswordHash = "admin123", // In real world use hashing
            Email = "admin@marketplace.com",
            FullName = "مدیر سیستم",
            Role = "Admin"
        };

        var user = new User
        {
            Username = "user",
            PasswordHash = "user123",
            Email = "user@marketplace.com",
            FullName = "کاربر نمونه",
            Role = "Customer"
        };

        context.Users.AddRange(admin, user);
        context.SaveChanges();
    }
}

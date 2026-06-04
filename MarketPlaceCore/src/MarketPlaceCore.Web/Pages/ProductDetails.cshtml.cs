using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlaceCore.Web.Pages;

public class ProductDetailsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductDetailsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Product? Product { get; set; }

    public async Task OnGetAsync(int id)
    {
        Product = await _unitOfWork.Products.GetByIdAsync(id);
        if (Product != null)
        {
            var variants = await _unitOfWork.ProductVariants.Find(v => v.ProductId == id);
            Product.Variants = variants.ToList();

            var reviews = await _unitOfWork.Reviews.Find(r => r.ProductId == id);
            // Include user names for reviews
            foreach(var r in reviews)
            {
                r.User = (await _unitOfWork.Users.Find(u => u.Id == r.UserId)).FirstOrDefault() ?? new User { FullName = "کاربر مهمان" };
            }
            Product.Reviews = reviews.ToList();
        }
    }
}

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
        var products = await _unitOfWork.Products.FindWithIncludes(p => p.Id == id, p => p.Variants, p => p.Category);
        Product = products.FirstOrDefault();

        if (Product != null)
        {
            var reviews = await _unitOfWork.Reviews.FindWithIncludes(r => r.ProductId == id, r => r.User);
            Product.Reviews = reviews.ToList();
        }
    }
}

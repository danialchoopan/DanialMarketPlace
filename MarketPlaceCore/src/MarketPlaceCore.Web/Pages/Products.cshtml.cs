using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlaceCore.Web.Pages;

public class ProductsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    public async Task OnGetAsync(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice)
    {
        Categories = await _unitOfWork.Categories.GetAllAsync();

        var products = await _unitOfWork.Products.GetAllAsync();
        // Include variants for UI calculations
        foreach(var p in products)
        {
            var pVariants = await _unitOfWork.ProductVariants.Find(v => v.ProductId == p.Id);
            p.Variants = pVariants.ToList();
        }

        if (!string.IsNullOrEmpty(search))
        {
            products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            ViewData["CurrentSearch"] = search;
        }

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        // Price filtering based on variants
        if (minPrice.HasValue)
        {
            products = products.Where(p => p.Variants.Any(v => v.Price >= minPrice.Value));
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(p => p.Variants.Any(v => v.Price <= maxPrice.Value));
        }

        Products = products.ToList();
    }
}

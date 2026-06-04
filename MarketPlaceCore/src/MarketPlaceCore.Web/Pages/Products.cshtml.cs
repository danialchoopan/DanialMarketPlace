using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
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

    public async Task OnGetAsync()
    {
        Products = await _unitOfWork.Products.GetAllAsync();
    }
}

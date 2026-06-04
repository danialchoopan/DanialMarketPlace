using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MarketPlaceCore.Web.Pages;
public class ProductDetailsModel : PageModel {
    private readonly IUnitOfWork _unitOfWork;
    public ProductDetailsModel(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public Product? Product { get; set; }
    public async Task OnGetAsync(int id) { Product = await _unitOfWork.Products.GetByIdAsync(id); }
}

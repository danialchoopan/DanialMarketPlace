using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarketPlaceCore.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    public async Task OnGetAsync()
    {
        Categories = await _unitOfWork.Categories.GetAllAsync();
    }
}

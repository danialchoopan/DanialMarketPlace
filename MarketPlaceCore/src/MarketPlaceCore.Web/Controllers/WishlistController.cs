using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using MarketPlaceCore.Web.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCore.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public WishlistController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("add/{productId}")]
    [Authorize]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
        var userIdStr = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
        int userId = int.Parse(userIdStr);

        var existing = (await _unitOfWork.Wishlist.Find(w => w.UserId == userId && w.ProductId == productId)).FirstOrDefault();
        if (existing != null) return Ok("در لیست علاقه‌مندی‌ها موجود است");

        var item = new WishlistItem { UserId = userId, ProductId = productId };
        await _unitOfWork.Wishlist.AddAsync(item);
        await _unitOfWork.CompleteAsync();
        return Ok("به لیست علاقه‌مندی‌ها اضافه شد");
    }

    [HttpPost("alert/{productId}")]
    [Authorize]
    public async Task<IActionResult> SetAlert(int productId)
    {
        var userIdStr = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
        int userId = int.Parse(userIdStr);

        var alert = new ProductAlert { UserId = userId, ProductId = productId, IsNotified = false };
        await _unitOfWork.ProductAlerts.AddAsync(alert);
        await _unitOfWork.CompleteAsync();
        return Ok("اطلاع‌رسانی موجودی فعال شد");
    }
}

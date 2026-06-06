using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using MarketPlaceCore.Web.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCore.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
    {
        var userIdStr = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
        int userId = int.Parse(userIdStr);

        // Check if user has bought this product (Enterprise Feature: Verified Buyer)
        var userOrders = await _unitOfWork.Orders.FindWithIncludes(
            o => o.UserId == userId && o.Status == OrderStatus.Delivered,
            o => o.OrderItems);
        bool isVerified = userOrders.Any(o => o.OrderItems.Any(oi => oi.ProductId == reviewDto.ProductId));

        var review = new Review
        {
            ProductId = reviewDto.ProductId,
            UserId = userId,
            Comment = reviewDto.Comment,
            Rating = reviewDto.Rating,
            IsVerifiedPurchase = isVerified,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Reviews.AddAsync(review);
        await _unitOfWork.CompleteAsync();

        return Ok(new { Message = "نظر شما ثبت شد", IsVerified = isVerified });
    }
}

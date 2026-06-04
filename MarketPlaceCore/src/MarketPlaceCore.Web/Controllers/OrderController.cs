using MarketPlaceCore.Core.Entities;
using MarketPlaceCore.Core.Interfaces;
using MarketPlaceCore.Web.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCore.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto orderDto)
    {
        var userIdStr = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
        int userId = int.Parse(userIdStr);

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var order = new Order
            {
                UserId = userId,
                TotalAmount = 0,
                Status = OrderStatus.Pending,
                AppliedCouponCode = orderDto.CouponCode
            };

            foreach (var item in orderDto.Items)
            {
                decimal unitPrice = 0;
                if (item.ProductVariantId.HasValue)
                {
                    var variant = await _unitOfWork.ProductVariants.GetByIdAsync(item.ProductVariantId.Value);
                    if (variant == null || variant.Stock < item.Quantity)
                        throw new Exception("موجودی تنوع انتخابی کافی نیست");

                    variant.Stock -= item.Quantity;
                    _unitOfWork.ProductVariants.Update(variant);
                    unitPrice = variant.Price;
                }
                else
                {
                    throw new Exception("انتخاب تنوع محصول الزامی است");
                }

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice
                };
                order.OrderItems.Add(orderItem);
                order.TotalAmount += (unitPrice * item.Quantity);
            }

            // Apply Coupon logic
            if (!string.IsNullOrEmpty(orderDto.CouponCode))
            {
                var coupon = (await _unitOfWork.Coupons.Find(c => c.Code == orderDto.CouponCode)).FirstOrDefault();
                if (coupon != null && coupon.ExpiryDate > DateTime.UtcNow && coupon.CurrentUsageCount < coupon.MaxUsageCount && order.TotalAmount >= coupon.MinimumOrderAmount)
                {
                    if (coupon.DiscountPercentage.HasValue)
                        order.DiscountAmount = order.TotalAmount * (coupon.DiscountPercentage.Value / 100);
                    else
                        order.DiscountAmount = coupon.DiscountAmount;

                    coupon.CurrentUsageCount++;
                    _unitOfWork.Coupons.Update(coupon);
                }
            }

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return Ok(new { OrderId = order.Id, FinalAmount = order.FinalAmount });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return BadRequest(ex.Message);
        }
    }
}

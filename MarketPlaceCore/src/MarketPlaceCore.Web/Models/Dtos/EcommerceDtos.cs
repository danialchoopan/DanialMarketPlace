namespace MarketPlaceCore.Web.Models.Dtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public List<ProductVariantDto> Variants { get; set; } = new();
    public double AverageRating { get; set; }
}

public class ProductVariantDto
{
    public int Id { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Warranty { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

public class ReviewDto
{
    public int ProductId { get; set; }
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; }
}

public class OrderRequestDto
{
    public List<OrderItemRequestDto> Items { get; set; } = new();
    public string? CouponCode { get; set; }
}

public class OrderItemRequestDto
{
    public int ProductId { get; set; }
    public int? ProductVariantId { get; set; }
    public int Quantity { get; set; }
}

public class CouponApplyDto
{
    public string Code { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

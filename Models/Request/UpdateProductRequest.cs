namespace cybersoft_final_project.Models.Request;

public class UpdateProductRequest
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string? Brand { get; set; }
    public string? Sku { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountPrice { get; set; }
    public int? stock_quantity { get; set; } // ← sửa từ int → int?
}

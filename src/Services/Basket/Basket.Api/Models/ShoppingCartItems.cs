namespace Basket.Api.Models;

public class ShoppingCartItems
{
    public Guid ProductId { get; set; } = Guid.Empty!;
    public int Quantity { get; set; }
    public string Color { get; set; } = null!;
    public decimal Price { get; set; } 
    public string ProductName { get; set; } = null!;

}
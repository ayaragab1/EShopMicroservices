namespace Basket.Api.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = null!;
    public List<ShoppingCartItems> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

    //required for Mapping
    public ShoppingCart()
    {
        
    }
}
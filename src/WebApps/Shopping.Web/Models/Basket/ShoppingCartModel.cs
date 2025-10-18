﻿namespace Shopping.Web.Models.Basket;

public class ShoppingCartModel
{
    public string UserName { get; set; } = null!;
    public List<ShoppingCartItemModel> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}

public class ShoppingCartItemModel
{
    public int Quantity { get; set; }
    public string Color { get; set; } = null!;
    public decimal Price { get; set; } 
    public Guid ProductId { get; set; } 
    public string ProductName { get; set; } = null!;
}

// wrapper classes
public record GetBasketResponse(ShoppingCartModel ShoppingCart);

public record StoreBasketRequest(ShoppingCartModel ShoppingCart);
public record StoreBasketResponse(string UserName);

public record DeleteBasketResponse(bool IsSuccess);
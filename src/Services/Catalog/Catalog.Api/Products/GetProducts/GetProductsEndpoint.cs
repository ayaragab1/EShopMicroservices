﻿namespace Catalog.Api.Products.GetProducts;

//public record GetProductsRequest;
public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint :ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products" , async (ISender sender) =>
        {
            var command = new GetProductsQuery();

            var result = await sender.Send(command);

            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response); 

        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Retrieves a list of products.");
    }
}
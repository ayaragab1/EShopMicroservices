namespace Catalog.Api.Products.GetProductById;

//public record GetProductByIdRequest(Guid ProductId);
public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndpoint :ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}" , async (Guid id, ISender sender) =>
        {
            var command =  new GetProductByIdQuery(id);
            var result = await sender.Send(command);
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);

        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product")
        .WithDescription("Retrieves a product.");
    }
}
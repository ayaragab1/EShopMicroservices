
namespace Catalog.Api.Products.GetProductById;

public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

public class GetProductByIdQueryHandler(IDocumentSession session , ILogger<GetProductByIdQuery> logger)
    :IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIdQueryHandler.Handler called with {@query}", query);
        
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
        if(product == null) 
            throw new ProductNotFoundException();
        
        return new GetProductByIdResult(product);
    }
}
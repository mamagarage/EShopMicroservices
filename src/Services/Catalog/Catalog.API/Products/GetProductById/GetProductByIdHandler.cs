namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);


public class GetProductByQueryIdHandler 
    (IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        //logger.LogInformation("GetProductByIdQueryHandler.Handle called with Id: {ProductId}", query.Id);

        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundExceptions(query.Id);
        }

        return new GetProductByIdResult(product);
    }
}

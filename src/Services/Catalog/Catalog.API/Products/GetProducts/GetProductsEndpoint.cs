namespace Catalog.API.Products.GetProducts;

public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var query = new GetProductsQuery();

            GetProductsResult getProductsResult = await sender.Send(query);
            
            var getProductResponse = getProductsResult.Adapt<GetProductResponse>();
            
            return Results.Ok(getProductResponse);
        })
        .WithName("GetProducts")
        .Produces<GetProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}
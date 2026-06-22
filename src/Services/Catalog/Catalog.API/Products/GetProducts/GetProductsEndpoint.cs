namespace Catalog.API.Products.GetProducts;

public record GetProductResponse(IEnumerable<Product> Products);

public record GetProductsRequest(int? PageNumber=1, int PageSize=10);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();

            var getProductsResult = await sender.Send(query);
            
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
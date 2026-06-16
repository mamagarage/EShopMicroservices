namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public Guid Id;

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", 
            async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();

            CreateProductResult createProductResult = await sender.Send(command);

            var createProductResponse = createProductResult.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{createProductResponse.Id}", createProductResponse);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
}
namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResponse(bool IsSuccess);


public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteProductCommand(id);

            var result = await sender.Send(command);

            var response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError)
        .WithSummary("Deletes an existing product")
        .WithDescription("Deletes an existing product with the provided details.");
    }
}
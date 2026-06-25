using MediatR;

namespace Basket.API.Basket.GetBasket;

//public record GetBasketRequest(string UserName); 
public record GetBasketResponse(ShoppingCart Cart);

// ICarterModule is from Carter, which is used to define the endpoint
public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // ISender is from MediatR, which is used to send the query to the handler
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));

            // Adapt from Mapster to map the result to the response
            var response = result.Adapt<GetBasketResponse>();

            return Results.Ok(response);
        })
        .WithName("GetBasketByUserName")
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Basket By User Name")
        .WithDescription("Get Basket By User Name");
    }
}
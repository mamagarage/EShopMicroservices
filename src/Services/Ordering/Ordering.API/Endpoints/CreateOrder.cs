using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

//- Accepts a CreateOrderRequest object.
//- Maps the request to a CreateOrderCommand.
//- Uses MediatR to send the command to the corresponding handler.
//- Returns a response with the created order's ID.

public record CreateOrderRequest(OrderDto Order);
public record CreateOrderResponse(Guid Id);

// What the benefits of using Carter in this code are:
// Carter is a library that simplifies the creation of HTTP endpoints in ASP.NET Core applications. The benefits of using Carter in this code include:
// 1. Simplified routing: Carter provides a clean and concise way to define routes and endpoints, making the code easier to read and maintain.
// 2. Minimal boilerplate: Carter reduces the amount of boilerplate code needed to set up endpoints, allowing developers to focus on the core logic of their application.
// 3. Integration with MediatR: Carter works well with MediatR, allowing for easy handling of commands and queries in a CQRS pattern.
// 4. Improved testability: By separating the endpoint definitions from the application logic, Carter makes it easier to write unit tests for the endpoints and their associated handlers.
// 5. Enhanced documentation: Carter allows for easy addition of metadata, such as summaries and descriptions, which can improve API documentation and help developers understand the purpose of each endpoint.
// Overall, using Carter in this code helps to create a more maintainable, testable, and well-documented API.
public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            // Map the incoming request to a CreateOrderCommand using Mapster
            var command = request.Adapt<CreateOrderCommand>();

            // Send the command to the MediatR pipeline and await the result
            var result = await sender.Send(command);

            // Map the result to a CreateOrderResponse using Mapster
            var response = result.Adapt<CreateOrderResponse>();

            // Return a 201 Created response with the location of the newly created order
            return Results.Created($"/orders/{response.Id}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Order")
        .WithDescription("Create Order");
    }
}

using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assembly = typeof(Program).Assembly;

// Add MediatR for handling commands and queries. MediatR is a library that implements the mediator pattern,
// allowing you to decouple the sender of a request from its handler.
// This promotes a clean architecture and separation of concerns.
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);

    // Add behaviors for validation and logging. These behaviors will be executed before the actual request handler is invoked.
    // ValidationBehavior will validate the request using FluentValidation, and LogginBehavior will log the request and response.
    // the sender is the mediator that sends the request to the handler, and the handler is the class that handles the request.
    // in this case the handler is the class that implements the ICommandHandler or IQueryHandler interface.
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);


// Add Carter for defining endpoints in a more functional style. Carter is a library
// that allows you to define routes and endpoints in a more concise and expressive way, making it easier to build APIs.
builder.Services.AddCarter();

// Add Marten
// Note: Ensure that the connection string "DataBase" is defined in your appsettings.json or environment variables
// Example connection string in appsettings.json:
// "ConnectionStrings": {
//     "DataBase": "Host=localhost;Port=5432;Database=CatalogDb;Username=postgres;Password=yourpassword"
// }
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("DataBase"));

    // Configure Marten to use the ShoppingCart class as a document type and set the UserName property as the identity (primary key) for the document.    
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName); 

}).UseLightweightSessions(); // UseLightweightSessions is used to configure Marten to use lightweight sessions, which are optimized for read operations and do not track changes to documents. This can improve performance for read-heavy workloads.


// Add custom exception handler middleware to handle exceptions globally in the application.
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Decorate the IBasketRepository with CachedBasketRepository to add caching functionality.
// This means that when an instance of IBasketRepository is requested, 
// an instance of CachedBasketRepository will be provided, which wraps the original BasketRepository and adds caching behavior.
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

// Add Redis cache for caching basket data. This allows the application to store and retrieve basket data from a Redis cache,
// improving performance and scalability.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket_";
});


//builder.Services.AddScoped<IBasketRepository, CachedBasketRepository>();

//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());

//});



// what purpose does this serve? This code configures a gRPC client for the DiscountProtoService. It sets the address of the gRPC service using the configuration value "GrpcSettings:DiscountUrl". The ConfigurePrimaryHttpMessageHandler method is used to configure the HTTP message handler for the gRPC client, allowing it to accept any server certificate (useful for development or testing environments with self-signed certificates).
// This setup enables the Basket API to communicate with the Discount gRPC service to retrieve discount information.
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")!);
    ;
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

// Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);


// Cross-cutting concerns: Add custom behaviors for validation and logging.
// These behaviors will be executed before the actual request handler is invoked,
// allowing you to perform validation and logging in a consistent manner across all requests.


// Add custom exception handler middleware to handle exceptions globally in the application.
// This middleware will catch exceptions thrown during the request processing pipeline and allow you to handle them in a centralized manner,
// providing consistent error responses to clients.
builder.Services.AddExceptionHandler<CustomExceptionHandler>();



builder.Services

    // Add health checks to monitor the health of the application and its dependencies.
    .AddHealthChecks()

    // Add PostgreSQL health check to monitor the health of the PostgreSQL database.
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)

    // Add Redis health check to monitor the health of the Redis cache.
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();



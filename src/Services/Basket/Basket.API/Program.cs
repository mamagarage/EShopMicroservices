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
    config.AddOpenBehavior(typeof(LogginBehavior<,>));
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


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//builder.Services.AddScoped<IBasketRepository, CachedBasketRepository>();

//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository = provider.GetRequiredService<BasketRepository>();
//    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());

//});

builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket_";
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services
    .AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DataBase")!)
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



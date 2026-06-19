var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LogginBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

// Add Marten
// Note: Ensure that the connection string "DataBase" is defined in your appsettings.json or environment variables
// Example connection string in appsettings.json:
// "ConnectionStrings": {
//     "DataBase": "Host=localhost;Port=5432;Database=CatalogDb;Username=postgres;Password=yourpassword"
// }
builder.Services.AddMarten(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DataBase");

    options.Connection(connectionString);
    //options.AutoCreateSchemaObjects = 
}).UseLightweightSessions(); // UseLightweightSessions is used to configure Marten to use lightweight sessions, which are optimized for read operations and do not track changes to documents. This can improve performance for read-heavy workloads.

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();


app.UseExceptionHandler(options =>{ });

/*
// Global exception handling middleware
app.UseExceptionHandler(exceptionHandlerApp => 
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception == null)
        {
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = exception.Message,
            Detail = exception.StackTrace
        };

        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, "An unhandled exception occurred.");

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

*/

app.Run();

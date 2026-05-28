var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Add Marten
// Note: Ensure that the connection string "DataBase" is defined in your appsettings.json or environment variables
// Example connection string in appsettings.json:
// "ConnectionStrings": {
//     "DataBase": "Host=localhost;Port=5432;Database=CatalogDb;Username=postgres;Password=yourpassword"
// }
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("DataBase"));
    //options.AutoCreateSchemaObjects = 
}).UseLightweightSessions(); // UseLightweightSessions is used to configure Marten to use lightweight sessions, which are optimized for read operations and do not track changes to documents. This can improve performance for read-heavy workloads.


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();


app.Run();

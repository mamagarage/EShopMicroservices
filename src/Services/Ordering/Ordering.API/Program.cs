using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    
    // Add Infrastructure services to the IServiceCollection using the extension method defined in the DependencyInjection class
    .AddInfrastructureServices(builder.Configuration)
    
    .AddApiServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}


app.Run();

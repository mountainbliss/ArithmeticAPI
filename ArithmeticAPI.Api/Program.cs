using ArithmeticAPI.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Register our ArithmeticService with the DI container
builder.Services.AddScoped<ArithmeticService>();

// Add controller support
builder.Services.AddControllers();

// Add OpenAPI support (built into .NET 10)
builder.Services.AddOpenApi();

var app = builder.Build();

// Enable OpenAPI in development mode
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

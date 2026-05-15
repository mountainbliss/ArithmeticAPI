using ArithmeticAPI.Core;
using ArithmeticAPI.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ── Register services ────────────────────────────────────────────────────────
builder.Services.AddSingleton<CalculationHistoryService>();
builder.Services.AddScoped<ArithmeticService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// ── Middleware pipeline ──────────────────────────────────────────────────────
// Order matters!

// 1. Our custom logging + error handling middleware
// Registered first so it wraps everything below
app.UseMiddleware<RequestLoggingMiddleware>();

// 2. Built-in middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();  // only redirect in production
}

app.UseAuthorization();
app.MapControllers();

app.Run();
using ArithmeticAPI.Core;
using Microsoft.AspNetCore.Mvc;
using ArithmeticAPI.Api.Middleware;
using ArithmeticAPI.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

// ── Register services ────────────────────────────────────────────────────────
builder.Services.AddSingleton<CalculationHistoryService>();
builder.Services.AddScoped<ArithmeticService>();
builder.Services.AddScoped<AuditLogFilter>();
builder.Services.AddScoped<ValidateModelFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelFilter>(); 
});

builder.Services.AddOpenApi();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddResponseCaching();

var app = builder.Build();

// ── Middleware pipeline ──────────────────────────────────────────────────────
// Order matters!

// 1. assign correlation ID first
// Registered first so it wraps everything below
app.UseMiddleware<CorrelationIdMiddleware>();  

// 2. Our custom logging + error handling middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// 3. measure timing
app.UseMiddleware<RequestTimingMiddleware>(); 

app.UseResponseCaching(); 

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
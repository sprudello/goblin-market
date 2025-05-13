using Microsoft.EntityFrameworkCore;
using goblin_api.Data; // Add this to use ProductDbContext
using Npgsql.EntityFrameworkCore.PostgreSQL; // Add this for UseNpgsql

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add DbContext configuration
builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(); // Add support for controllers
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply migrations automatically on startup (for development)
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    try {
        var context = services.GetRequiredService<ProductDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex) {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// UseHttpsRedirection can cause issues in Docker environment
// Comment out for local development with Docker
// app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Removed the inline summaries array and MapGet("/weatherforecast", ...) logic

app.MapControllers(); // Map attribute-routed controllers

app.Run();

// Removed WeatherForecast record definition from here

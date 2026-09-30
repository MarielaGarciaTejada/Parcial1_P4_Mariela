using Scalar.AspNetCore;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<NumbersService>();

var app = builder.Build();

//Inicializa la tabla SQLite al arrancar
using (var scope = app.Services.CreateScope())
{
    var numbersService = scope.ServiceProvider.GetRequiredService<NumbersService>();
    await numbersService.InitializeAsync();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Scalar;
using ApiPrestamoLibro.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<LibroServices>();
builder.Services.AddScoped<EstudianteServices>();
builder.Services.AddScoped<PrestamoServices>();
builder.Services.AddScoped<DevolucionServices>();
builder.Services.AddScoped<UsuarioBServices>();
builder.Services.AddScoped<BibliotecaServices>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    await services.GetRequiredService<LibroServices>().InitializeAsync();
    await services.GetRequiredService<EstudianteServices>().InitializeAsync();
    await services.GetRequiredService<PrestamoServices>().InitializeAsync();
    await services.GetRequiredService<DevolucionServices>().InitializeAsync();
    await services.GetRequiredService<UsuarioBServices>().InitializeAsync();
    await services.GetRequiredService<BibliotecaServices>().InitializeAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

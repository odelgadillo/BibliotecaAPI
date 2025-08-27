using System.Text.Json.Serialization;
using BibliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//area de srvicios

builder.Services.AddControllers().AddJsonOptions(opciones => opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

// area de middlewares (software intermedio)

app.Use(async (contexto, next) =>
{
    // Se realiza la solicitud al servidor
    var logger = contexto.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Petición: {contexto.Request.Method} {contexto.Request.Path}");

    await next.Invoke();

    // El servidor responde la solicitud
    logger.LogInformation($"Respuesta: {contexto.Response.StatusCode}");
});

app.MapControllers();

app.Run();

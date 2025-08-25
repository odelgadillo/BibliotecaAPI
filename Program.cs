var builder = WebApplication.CreateBuilder(args);

//area de srvicios

builder.Services.AddControllers();

var app = builder.Build();

// area de middlewares (software intermedio)

app.MapControllers();

app.Run();

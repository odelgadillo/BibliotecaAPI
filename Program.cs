using System.Text.Json.Serialization;
using BibliotecaAPI;
using BibliotecaAPI.Datos;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var diccionarioConfiguraciones = new Dictionary<string, string>
{
    {"quien_soy", "un dicccionario en memoria"}
};

builder.Configuration.AddInMemoryCollection(diccionarioConfiguraciones!);

//area de srvicios

builder.Services.AddOptions<PersonaOpciones>()
    .Bind(builder.Configuration.GetSection(PersonaOpciones.Seccion))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddAutoMapper(cfg =>{ }, typeof(Program).Assembly);
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

// area de middlewares (software intermedio)

app.MapControllers();

app.Run();

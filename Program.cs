using System.Text;
using System.Text.Json.Serialization;
using BibliotecaAPI;
using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Servicios;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

//area de servicios

builder.Services.AddDataProtection();

var origenesPermitidos = builder.Configuration.GetSection("origenesPermitidos").Get<string[]>()!;
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(origenesPermitidos).AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders("mi-cabecera");
    });
});

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection"));



builder.Services.AddIdentityCore<Usuario>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<Usuario>>();        //Manejadro de usuarios para registrar usuarios
builder.Services.AddScoped<SignInManager<Usuario>>();      //Permite autenticar usuarios
builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>(); // Para obtener el usuario logeado

builder.Services.AddHttpContextAccessor();                      //Permite acceder al contexto http desde cualquier clase
builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false; //Para que ASP.Net Core no cambie el nombre de un claim por otro de forma automatica
    opciones.TokenValidationParameters = new TokenValidationParameters //Define que se va a tomar en cuenta al validar un token
    {
        ValidateIssuer = false,             //Valida el emisor del token
        ValidateAudience = false,           //Valida la audiencia
        ValidateLifetime = true,            //Valida el tiempo de vida del token
        ValidateIssuerSigningKey = true,    //valida la llave secreta
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),    // Configurar llave secreta
        ClockSkew = TimeSpan.Zero           //Para no tener problemas de discrepancia temporal
    };
});

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin", "true"));
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// area de middlewares (software intermedio)

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();

app.Run();

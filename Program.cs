using System.Text;
using System.Text.Json.Serialization;
using BibliotecaAPI;
using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Servicios;
using BibliotecaAPI.Swagger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

//area de servicios

builder.Services.AddDataProtection();

var origenesPermitidos = builder.Configuration.GetSection("origenesPermitidos").Get<string[]>()!;
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(opcionesCORS =>
    {
        opcionesCORS.WithOrigins(origenesPermitidos).AllowAnyMethod().AllowAnyHeader()
        .WithExposedHeaders("cantidad-total-registros");
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

builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Biblioteca API",
        Description = "Este es un web api para trabajar con datos de autores y libros",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Email = "odelgadillo@outlook.com",
            Name = "Omar Delgadillo",
            Url = new Uri("https://odelgadillo.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/license/mit")
        }
    });

    opciones.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    opciones.OperationFilter<FiltroAutorizacion>();

    // opciones.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {    
    //     {
    //         new OpenApiSecurityScheme{
    //             Reference = new OpenApiReference{
    //                 Type = ReferenceType.SecurityScheme,
    //                 Id = "Bearer"
    //             }
    //         },
    //         new string[]{ }
    //     }
    // });

});

var app = builder.Build();

// area de middlewares (software intermedio)

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();

app.Run();

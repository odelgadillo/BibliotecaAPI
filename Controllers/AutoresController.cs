using System;
using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public AutoresController(ApplicationDbContext context)
    {
        this.context = context;
    }


    [HttpGet]
    public IEnumerable<Autor> Get()
    {
        return new List<Autor>
        {
            new Autor{Id=1, Nombre="Felipe" },
            new Autor{Id=2, Nombre="Omar"}
        };
    }

    [HttpPost]
    public async Task<ActionResult> Post(Autor autor)
    {
        context.Add(autor);
        await context.SaveChangesAsync();
        return Ok();
    }
}

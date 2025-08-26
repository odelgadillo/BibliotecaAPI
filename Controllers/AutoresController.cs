using System;
using BibliotecaAPI.Datos;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IEnumerable<Autor>> Get()
    {
        return await context.Autores.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Autor>> Get(int id)
    {
        var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
        if (autor is null)
        {
            return NotFound();
        }

        return autor;
    }

    [HttpPost]
    public async Task<ActionResult> Post(Autor autor)
    {
        context.Add(autor);
        await context.SaveChangesAsync();
        return Ok();
    }
}

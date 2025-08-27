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

    [HttpGet("{id:int}", Name = "ObtenerAutor")]
    public async Task<ActionResult<Autor>> Get(int id)
    {
        var autor = await context.Autores
            .Include(x => x.Libros)
            .FirstOrDefaultAsync(x => x.Id == id);
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
        return CreatedAtRoute("ObtenerAutor", new { id = autor.Id}, autor);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, Autor autor)
    {
        if (autor.Id != id)
        {
            return BadRequest("El id del autor no coincide con el id de la URL.");
        }

        var existe = await context.Autores.AnyAsync(x => x.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        context.Update(autor);
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var registrosBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (registrosBorrados == 0)
        {
            return NotFound();
        }

        return Ok();
    }
}

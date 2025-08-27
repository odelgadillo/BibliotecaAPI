using System;
using AutoMapper;
using BibliotecaAPI.Datos;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/autores")]
public class AutoresController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public AutoresController(ApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }


    [HttpGet] // api/autores
    public async Task<IEnumerable<AutorDTO>> Get()
    {
        var autores = await context.Autores.ToListAsync();
        var autoresDTO = mapper.Map<IEnumerable<AutorDTO>>(autores);

        return autoresDTO;
    }

    [HttpGet("{id:int}", Name = "ObtenerAutor")]
    public async Task<ActionResult<AutorConLibrosDTO>> Get(int id)
    {
        var autor = await context.Autores
            .Include(x => x.Libros)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (autor is null)
        {
            return NotFound();
        }
        var autorDTO = mapper.Map<AutorConLibrosDTO>(autor);
        return autorDTO;
    }

    [HttpPost]
    public async Task<ActionResult> Post(AutorCreacionDTO autorCreacionDTO)
    {
        var autor = mapper.Map<Autor>(autorCreacionDTO);
        context.Add(autor);
        await context.SaveChangesAsync();
        var autorDTO = mapper.Map<AutorDTO>(autor);
        return CreatedAtRoute("ObtenerAutor", new { id = autor.Id }, autorDTO);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, AutorCreacionDTO autorCreacionDTO)
    {
        var autor = mapper.Map<Autor>(autorCreacionDTO);  
        var existe = await context.Autores.AnyAsync(x => x.Id == id);
        if (!existe)
            return NotFound();
        autor.Id = id;

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

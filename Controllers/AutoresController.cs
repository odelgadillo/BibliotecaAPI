using System;
using System.ComponentModel;
using AutoMapper;
using BibliotecaAPI.Datos;
using BibliotecaAPI.DTOs;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers;

[ApiController]
[Route("api/autores")]
[Authorize(Policy = "esadmin")]
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
    [AllowAnonymous]
    public async Task<IEnumerable<AutorDTO>> Get([FromQuery] PaginacionDTO paginacionDTO)
    {
        var queryable = context.Autores.AsQueryable();
        await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
        var autores = await queryable
                            .OrderBy(x => x.Nombres)
                            .Paginar(paginacionDTO)
                            .ToListAsync();
        var autoresDTO = mapper.Map<IEnumerable<AutorDTO>>(autores);

        return autoresDTO;
    }

    [HttpGet("{id:int}", Name = "ObtenerAutor")]
    [AllowAnonymous]
    [EndpointSummary("Obtiene autor por Id")]
    [EndpointDescription("Obtiene un autor por su Id. Incluye sus libros. Si el auto no existe, se retorna 404.")]
    [ProducesResponseType<AutorConLibrosDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AutorConLibrosDTO>> Get([Description("El id del autor")] int id)
    {
        var autor = await context.Autores
            .Include(x => x.Libros)
                .ThenInclude(x => x.Libro)
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

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, JsonPatchDocument<AutorPatchDTO> patchDoc)
    {
        if (patchDoc is null)
        {
            return BadRequest();
        }

        var autorDB = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
        if (autorDB is null)
        {
            return NotFound();
        }

        var autorPatchDTO = mapper.Map<AutorPatchDTO>(autorDB);
        patchDoc.ApplyTo(autorPatchDTO, ModelState);

        var esValido = TryValidateModel(autorPatchDTO);
        if (!esValido)
        {
            return ValidationProblem();
        }

        mapper.Map(autorPatchDTO, autorDB);
        await context.SaveChangesAsync();

        return NoContent();
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
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var registrosBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (registrosBorrados == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}

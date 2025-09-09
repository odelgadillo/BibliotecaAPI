using System;
using BibliotecaAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Datos;

public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Libro> Libros { get; set; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet <AutorLibro> AutoresLibros { get; set; }        
}

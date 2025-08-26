using System.ComponentModel.DataAnnotations;
using BibliotecaAPI.Entidades;

namespace BibliotecaAPI;

public class Libro
{
    public int Id { get; set; }

    [Required]
    public required string Titulo { get; set; }

    public int AutorId { get; set; }
    public Autor? Autor { get; set; }
}

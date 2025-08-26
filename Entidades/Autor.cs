using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Entidades;

public class Autor
{
    public int Id { get; set; }

    [Required]
    [StringLength(150, ErrorMessage ="El campo {0} debe tener {1} caracteres o menos")]
    public required string Nombre { get; set; }

    public List<Libro> Libros { get; set; } = new();
}

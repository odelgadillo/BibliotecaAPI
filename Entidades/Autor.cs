using System.ComponentModel.DataAnnotations;
using BibliotecaAPI.Validaciones;

namespace BibliotecaAPI.Entidades;

public class Autor
{
    public int Id { get; set; }

    [Required]
    [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} caracteres o menos")]
    [PrimeraLetraMayuscula]
    public required string Nombre { get; set; }

    public List<Libro> Libros { get; set; } = new();
}

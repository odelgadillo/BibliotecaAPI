using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI;

public class TarifaOpciones
{
    public const string seccion = "tarifas";

    [Required]
    public decimal dia { get; set; }

    [Required]
    public decimal noche { get; set; }

}

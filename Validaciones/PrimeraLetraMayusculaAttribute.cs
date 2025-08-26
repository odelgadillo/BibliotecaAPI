using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaAPI.Validaciones;

public class PrimeraLetraMayusculaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var valueString = value.ToString()!;
        var primerLetra = valueString[0].ToString();

        if (primerLetra != primerLetra.ToUpper())
        {
            return new ValidationResult("La primera letra debe ser mayuscula");
        }

        return ValidationResult.Success;
    }
}

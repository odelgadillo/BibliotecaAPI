using System;

namespace BibliotecaAPI.DTOs;

public record PaginacionDTO(int Pagina = 1, int RecordsPorPagina = 10)
{
    private const int CantidadMaximaRecordsPorPagina = 50;
    public int Pagina { get; set; } = Math.Max(1, Pagina);
    public int RecordsPorPagina { get; set; } = Math.Clamp(RecordsPorPagina, 1, CantidadMaximaRecordsPorPagina);
}

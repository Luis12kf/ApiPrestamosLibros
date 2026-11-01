namespace ApiPrestamoLibro.Models;

public record LibroRecordSet(
    string? Titulo,
    string? Autor,
    string? Isbn,
    int? CantidadTotal
);

public record LibroRecordGet
{
    public long IdLibro { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string Autor { get; init; } = string.Empty;
    public string Isbn { get; init; } = string.Empty;
    public int CantidadTotal { get; init; }
}
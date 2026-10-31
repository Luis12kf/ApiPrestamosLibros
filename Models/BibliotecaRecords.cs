namespace ApiPrestamoLibro.Models;

public record BibliotecaRecordSet(
    string? Direccion
);

public record BibliotecaRecordGet
{
    public long BibliotecaID { get; init; }
    public string Direccion { get; init; } = string.Empty;
}
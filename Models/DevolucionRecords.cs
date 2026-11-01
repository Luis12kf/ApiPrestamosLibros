namespace ApiPrestamoLibro.Models;

public record DevolucionRecordSet(
    long? PrestamoID,
    string? FechaDevolucion,
    string? Observaciones
);

public record DevolucionRecordGet
{
    public long IdDevolucion { get; init; }
    public long PrestamoID { get; init; }
    public string FechaDevolucion { get; init; } = string.Empty;
    public string Observaciones { get; init; } = string.Empty;
}
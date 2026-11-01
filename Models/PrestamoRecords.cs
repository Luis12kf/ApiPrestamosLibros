namespace ApiPrestamoLibro.Models;

public record PrestamoRecordSet(
    long? IdLibro,
    long? IdEstudiante,
    string? FechaPrestamo,
    string? FechaVencimiento
);

public record PrestamoRecordGet
{
    public long IdPrestamo { get; init; }
    public long IdLibro { get; init; }
    public long IdEstudiante { get; init; }
    public string FechaPrestamo { get; init; } = string.Empty;
    public string FechaVencimiento { get; init; } = string.Empty;
}
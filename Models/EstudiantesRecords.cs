namespace ApiPrestamoLibro.Models;

public record EstudianteRecordSet(
    string? Nombre,
    string? Apellido,
    string? Matricula,
    string? Celular,
    string? Email
);

public record EstudianteRecordGet
{
    public long IdEstudiante { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Apellido { get; init; } = string.Empty;
    public string Matricula { get; init; } = string.Empty;
    public string Celular { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
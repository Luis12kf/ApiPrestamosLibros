namespace ApiPrestamoLibro.Models;

public record UsuarioBRecordSet(
    string? NombreU,
    string? ContraseNa
);

public record UsuarioBRecordGet
{
    public long UsuarioID { get; init; }
    public string NombreU { get; init; } = string.Empty;
    public string ContraseNa { get; init; } = string.Empty;
}
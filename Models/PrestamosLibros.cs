namespace PrestamosLibros.Models;

// ==========================================
// LIBROS
// ==========================================
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

// ==========================================
// ESTUDIANTES
// ==========================================
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

// ==========================================
// PRESTAMOS
// ==========================================
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
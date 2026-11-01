using Dapper;
using ApiPrestamoLibro.Models;
using Microsoft.Data.Sqlite;

namespace ApiPrestamoLibro.Services;

public class PrestamoServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS Prestamos (
                IdPrestamo INTEGER PRIMARY KEY AUTOINCREMENT,
                IdLibro INTEGER,
                IdEstudiante INTEGER,
                FechaPrestamo TEXT,
                FechaVencimiento TEXT
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(PrestamoRecordSet prestamo)
    {
        const string query = @"INSERT INTO Prestamos (IdLibro, IdEstudiante, FechaPrestamo, FechaVencimiento)
                               VALUES (@IdLibro, @IdEstudiante, @FechaPrestamo, @FechaVencimiento)";

        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, prestamo);
        return filasAfectadas > 0;
    }

    public async Task<PrestamoRecordGet?> UpdateAsync(int id, PrestamoRecordSet prestamo)
    {
        const string query = @"
            UPDATE Prestamos
            SET IdLibro = COALESCE(@IdLibro, IdLibro),
                IdEstudiante = COALESCE(@IdEstudiante, IdEstudiante),
                FechaPrestamo = COALESCE(@FechaPrestamo, FechaPrestamo),
                FechaVencimiento = COALESCE(@FechaVencimiento, FechaVencimiento)
            WHERE IdPrestamo = @Id";

        using var conexion = CreateConnection();
        var filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            IdLibro = prestamo.IdLibro == 0 ? (long?)null : prestamo.IdLibro,
            IdEstudiante = prestamo.IdEstudiante == 0 ? (long?)null : prestamo.IdEstudiante,
            FechaPrestamo = string.IsNullOrWhiteSpace(prestamo.FechaPrestamo) ? null : prestamo.FechaPrestamo,
            FechaVencimiento = string.IsNullOrWhiteSpace(prestamo.FechaVencimiento) ? null : prestamo.FechaVencimiento
        });

        return filasAfectadas > 0 ? new PrestamoRecordGet { IdPrestamo = id } : null;
    }

    public async Task<PrestamoRecordGet?> DeleteAsync(int id)
    {
        const string query = "DELETE FROM Prestamos WHERE IdPrestamo = @Id";
        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id });
        return filasAfectadas > 0 ? new PrestamoRecordGet { IdPrestamo = id } : null;
    }

    public async Task<PrestamoRecordGet?> GetByIdAsync(int id)
    {
        const string query = "SELECT IdPrestamo, IdLibro, IdEstudiante, FechaPrestamo, FechaVencimiento FROM Prestamos WHERE IdPrestamo = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<PrestamoRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<PrestamoRecordGet>> GetListAsync()
    {
        const string query = "SELECT IdPrestamo, IdLibro, IdEstudiante, FechaPrestamo, FechaVencimiento FROM Prestamos";
        using var conexion = CreateConnection();
        return await conexion.QueryAsync<PrestamoRecordGet>(query);
    }
}
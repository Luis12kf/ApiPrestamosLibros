using Dapper;
using ApiPrestamoLibro .Models;
using Microsoft.Data.Sqlite;

namespace ApiPrestamoLibro.Services;

public class DevolucionServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS Devolucion (
                IdDevolucion INTEGER PRIMARY KEY AUTOINCREMENT,
                PrestamoID INTEGER,
                FechaDevolucion TEXT,
                Observaciones TEXT
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(DevolucionRecordSet devolucion)
    {
        const string query = @"INSERT INTO Devolucion (PrestamoID, FechaDevolucion, Observaciones)
                               VALUES (@PrestamoID, @FechaDevolucion, @Observaciones)";

        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, devolucion);
        return filasAfectadas > 0;
    }

    public async Task<DevolucionRecordGet?> UpdateAsync(int id, DevolucionRecordSet devolucion)
    {
        const string query = @"
            UPDATE Devolucion
            SET PrestamoID = COALESCE(@PrestamoID, PrestamoID),
                FechaDevolucion = COALESCE(@FechaDevolucion, FechaDevolucion),
                Observaciones = COALESCE(@Observaciones, Observaciones)
            WHERE IdDevolucion = @Id";

        using var conexion = CreateConnection();
        var filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            PrestamoID = devolucion.PrestamoID == 0 ? (long?)null : devolucion.PrestamoID,
            FechaDevolucion = string.IsNullOrWhiteSpace(devolucion.FechaDevolucion) ? null : devolucion.FechaDevolucion,
            Observaciones = string.IsNullOrWhiteSpace(devolucion.Observaciones) ? null : devolucion.Observaciones
        });

        return filasAfectadas > 0 ? new DevolucionRecordGet { IdDevolucion = id } : null;
    }

    public async Task<DevolucionRecordGet?> DeleteAsync(int id)
    {
        const string query = "DELETE FROM Devolucion WHERE IdDevolucion = @Id";
        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id });
        return filasAfectadas > 0 ? new DevolucionRecordGet { IdDevolucion = id } : null;
    }

    public async Task<DevolucionRecordGet?> GetByIdAsync(int id)
    {
        const string query = "SELECT IdDevolucion, PrestamoID, FechaDevolucion, Observaciones FROM Devolucion WHERE IdDevolucion = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<DevolucionRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<DevolucionRecordGet>> GetListAsync()
    {
        const string query = "SELECT IdDevolucion, PrestamoID, FechaDevolucion, Observaciones FROM Devolucion";
        using var conexion = CreateConnection();
        return await conexion.QueryAsync<DevolucionRecordGet>(query);
    }
}
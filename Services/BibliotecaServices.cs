using Dapper;
using ApiPrestamoLibro.Models;
using Microsoft.Data.Sqlite;

namespace ApiPrestamoLibro.Services;

public class BibliotecaServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS Biblioteca (
                BibliotecaID INTEGER PRIMARY KEY AUTOINCREMENT,
                Direccion TEXT
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(BibliotecaRecordSet biblioteca)
    {
        const string query = @"INSERT INTO Biblioteca (Direccion) VALUES (@Direccion)";

        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, biblioteca);
        return filasAfectadas > 0;
    }

    public async Task<BibliotecaRecordGet?> UpdateAsync(int id, BibliotecaRecordSet biblioteca)
    {
        const string query = @"
            UPDATE Biblioteca
            SET Direccion = COALESCE(@Direccion, Direccion)
            WHERE BibliotecaID = @Id";

        using var conexion = CreateConnection();
        var filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            Direccion = string.IsNullOrWhiteSpace(biblioteca.Direccion) ? null : biblioteca.Direccion
        });

        return filasAfectadas > 0 ? new BibliotecaRecordGet { BibliotecaID = id } : null;
    }

    public async Task<BibliotecaRecordGet?> DeleteAsync(int id)
    {
        const string query = "DELETE FROM Biblioteca WHERE BibliotecaID = @Id";
        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id });
        return filasAfectadas > 0 ? new BibliotecaRecordGet { BibliotecaID = id } : null;
    }

    public async Task<BibliotecaRecordGet?> GetByIdAsync(int id)
    {
        const string query = "SELECT BibliotecaID, Direccion FROM Biblioteca WHERE BibliotecaID = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<BibliotecaRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<BibliotecaRecordGet>> GetListAsync()
    {
        const string query = "SELECT BibliotecaID, Direccion FROM Biblioteca";
        using var conexion = CreateConnection();
        return await conexion.QueryAsync<BibliotecaRecordGet>(query);
    }
}
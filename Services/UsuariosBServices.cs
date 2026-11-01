using Dapper;
using ApiPrestamoLibro.Models;
using Microsoft.Data.Sqlite;

namespace ApiPrestamoLibro.Services;

public class UsuarioBServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS UsuariosB (
                UsuarioID INTEGER PRIMARY KEY AUTOINCREMENT,
                NombreU TEXT,
                ContraseNa TEXT
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(UsuarioBRecordSet usuario)
    {
        const string query = @"INSERT INTO UsuariosB (NombreU, ContraseNa)
                               VALUES (@NombreU, @ContraseNa)";

        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, usuario);
        return filasAfectadas > 0;
    }

    public async Task<UsuarioBRecordGet?> UpdateAsync(int id, UsuarioBRecordSet usuario)
    {
        const string query = @"
            UPDATE UsuariosB
            SET NombreU = COALESCE(@NombreU, NombreU),
                ContraseNa = COALESCE(@ContraseNa, ContraseNa)
            WHERE UsuarioID = @Id";

        using var conexion = CreateConnection();
        var filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            NombreU = string.IsNullOrWhiteSpace(usuario.NombreU) ? null : usuario.NombreU,
            ContraseNa = string.IsNullOrWhiteSpace(usuario.ContraseNa) ? null : usuario.ContraseNa
        });

        return filasAfectadas > 0 ? new UsuarioBRecordGet { UsuarioID = id } : null;
    }

    public async Task<UsuarioBRecordGet?> DeleteAsync(int id)
    {
        const string query = "DELETE FROM UsuariosB WHERE UsuarioID = @Id";
        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id });
        return filasAfectadas > 0 ? new UsuarioBRecordGet { UsuarioID = id } : null;
    }

    public async Task<UsuarioBRecordGet?> GetByIdAsync(int id)
    {
        const string query = "SELECT UsuarioID, NombreU, ContraseNa FROM UsuariosB WHERE UsuarioID = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<UsuarioBRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<UsuarioBRecordGet>> GetListAsync()
    {
        const string query = "SELECT UsuarioID, NombreU, ContraseNa FROM UsuariosB";
        using var conexion = CreateConnection();
        return await conexion.QueryAsync<UsuarioBRecordGet>(query);
    }
}
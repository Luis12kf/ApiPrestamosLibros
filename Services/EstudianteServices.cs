using Dapper;
using ApiPrestamoLibro.Models;
using Microsoft.Data.Sqlite;

namespace ApiPrestamoLibro.Services;

public class EstudianteServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS Estudiantes (
                IdEstudiante INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre TEXT,
                Apellido TEXT,
                Matricula TEXT,
                Celular TEXT,
                Email TEXT
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(EstudianteRecordSet estudiante)
    {
        const string query = @"INSERT INTO Estudiantes (Nombre, Apellido, Matricula, Celular, Email)
                               VALUES (@Nombre, @Apellido, @Matricula, @Celular, @Email)";

        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, estudiante);
        return filasAfectadas > 0;
    }

    public async Task<EstudianteRecordGet?> UpdateAsync(int id, EstudianteRecordSet estudiante)
    {
        const string query = @"
            UPDATE Estudiantes
            SET Nombre = COALESCE(@Nombre, Nombre),
                Apellido = COALESCE(@Apellido, Apellido),
                Matricula = COALESCE(@Matricula, Matricula),
                Celular = COALESCE(@Celular, Celular),
                Email = COALESCE(@Email, Email)
            WHERE IdEstudiante = @Id";

        using var conexion = CreateConnection();
        var filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            Nombre = string.IsNullOrWhiteSpace(estudiante.Nombre) ? null : estudiante.Nombre,
            Apellido = string.IsNullOrWhiteSpace(estudiante.Apellido) ? null : estudiante.Apellido,
            Matricula = string.IsNullOrWhiteSpace(estudiante.Matricula) ? null : estudiante.Matricula,
            Celular = string.IsNullOrWhiteSpace(estudiante.Celular) ? null : estudiante.Celular,
            Email = string.IsNullOrWhiteSpace(estudiante.Email) ? null : estudiante.Email
        });

        return filasAfectadas > 0 ? new EstudianteRecordGet { IdEstudiante = id } : null;
    }

    public async Task<EstudianteRecordGet?> DeleteAsync(int id)
    {
        const string query = "DELETE FROM Estudiantes WHERE IdEstudiante = @Id";
        using var conexion = CreateConnection();
        int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id });
        return filasAfectadas > 0 ? new EstudianteRecordGet { IdEstudiante = id } : null;
    }

    public async Task<EstudianteRecordGet?> GetByIdAsync(int id)
    {
        const string query = "SELECT IdEstudiante, Nombre, Apellido, Matricula, Celular, Email FROM Estudiantes WHERE IdEstudiante = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<EstudianteRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<EstudianteRecordGet>> GetListAsync()
    {
        const string query = "SELECT IdEstudiante, Nombre, Apellido, Matricula, Celular, Email FROM Estudiantes";
        using var conexion = CreateConnection();
        return await conexion.QueryAsync<EstudianteRecordGet>(query);
    }
}
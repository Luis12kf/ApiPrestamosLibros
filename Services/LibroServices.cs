using Dapper;
using ApiPrestamoLibro.Models;
using Microsoft.Data.Sqlite;

namespace ApiPrestamoLibro.Services;

public class LibroServices(IConfiguration configuration)
{
    readonly IConfiguration _configuration = configuration;

    private SqliteConnection CreateConnection() =>
        new SqliteConnection(_configuration.GetConnectionString("DbSqlite_Ds"));

    public async Task InitializeAsync()
    {
        const string query = @"
            CREATE TABLE IF NOT EXISTS Libros (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Titulo TEXT,
                Autor TEXT,
                Isbn TEXT,
                CantidadTotal INTEGER
            );";

        using var conexion = CreateConnection();
        await conexion.ExecuteAsync(query);
    }

    public async Task<bool> SaveAsync(LibroRecordSet libro)
    {
        const string query = @"INSERT INTO Libros (Titulo, Autor, Isbn, CantidadTotal)
                               VALUES (@Titulo, @Autor, @Isbn, @CantidadTotal)";

        using var conexion = CreateConnection();

        var parametros = new
        {
            Titulo = libro.Titulo,
            Autor = libro.Autor,
            Isbn = libro.Isbn,
            CantidadTotal = libro.CantidadTotal
        };

        int filasAfectadas = await conexion.ExecuteAsync(query, parametros);
        return filasAfectadas > 0;
    }

    public async Task<LibroRecordGet?> UpdateAsync(int id, LibroRecordSet libro)
    {
        const string query = @"
            UPDATE Libros
            SET Titulo = COALESCE(@Titulo, Titulo),
                Autor = COALESCE(@Autor, Autor),
                Isbn = COALESCE(@Isbn, Isbn),
                CantidadTotal = COALESCE(@CantidadTotal, CantidadTotal)
            WHERE Id = @Id";

        using var conexion = CreateConnection();

        var filasAfectadas = await conexion.ExecuteAsync(query, new {
            Id = id,
            Titulo = string.IsNullOrWhiteSpace(libro.Titulo) ? null : libro.Titulo,
            Autor = string.IsNullOrWhiteSpace(libro.Autor) ? null : libro.Autor,
            Isbn = string.IsNullOrWhiteSpace(libro.Isbn) ? null : libro.Isbn,
            CantidadTotal = libro.CantidadTotal == 0 ? (int?)null : libro.CantidadTotal
        });

        return filasAfectadas > 0 ? new LibroRecordGet
        {
            IdLibro = id
        } : null;
    }

    public async Task<LibroRecordGet?> DeleteAsync(int id)
    {
        const string query = @"
            DELETE FROM Libros
            WHERE Id = @Id";

        using var conexion = CreateConnection();

        int filasAfectadas = await conexion.ExecuteAsync(query, new { Id = id });
        return filasAfectadas > 0 ? new LibroRecordGet
        {
            IdLibro = id
        } : null;
    }

    public async Task<LibroRecordGet?> GetByIdAsync(int id)
    {
        const string query = @"
            SELECT Id AS IdLibro, Titulo, Autor, Isbn, CantidadTotal
            FROM Libros WHERE Id = @Id";
        using var conexion = CreateConnection();
        return await conexion.QueryFirstOrDefaultAsync<LibroRecordGet>(query, new { Id = id });
    }

    public async Task<IEnumerable<LibroRecordGet>> GetListAsync()
    {
        const string query = "SELECT Id AS IdLibro, Titulo, Autor, Isbn, CantidadTotal FROM Libros";

        using var conexion = CreateConnection();
        return await conexion.QueryAsync<LibroRecordGet>(query);
    }
}
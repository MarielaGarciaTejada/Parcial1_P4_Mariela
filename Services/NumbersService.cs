using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using WebApi.Models;

namespace WebApi.Services;

// Primary constructor
public class NumbersService (IConfiguration configuration)
{
    //cadena de conexión
    private readonly string _connectionString = configuration.GetConnectionString("SqlLiteConnection")
        ?? throw new InvalidOperationException("No se encontro la cadena SqlLiteConnection ");
    
    private IDbConnection CreateConnection() => new SqliteConnection(_connectionString);

    public async Task InitializeAsync ()
    {
        const string consulta =@"
            CREATE TABLE IF NOT EXISTS NumberRecords
            (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Numero INTEGER NOT NULL,
                Resultado INTEGER NOT NULL,
                Fecha TEXT NOT NULL
            );
        ";


        using var connection = CreateConnection();
        await connection.ExecuteAsync(consulta);
    }

    public async Task <int> SaveAsync(NumberRecordSet record)
    {
        const string consulta=@"
        INSERT INTO NumberRecords (Numero, Resultado, Fecha)
        VALUES (@Numero, @Resultado, DATETIME('now'));
        SELECT last_insert_rowid();
        ";

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(consulta, record);
    }

    public async Task <bool> UpdateAsync(int id, NumberRecordSet record)
    {
        const string consulta = @"
            UPDATE NumberRecords
            SET Numero = @Numero,
            Resultado = @Resultado,
            Fecha = DATETIME('now')
            WHERE Id = @Id;
        ";

        using var connection = CreateConnection();
        var filasAfectadas = await connection.ExecuteAsync(consulta, new
        {
            Id = id,
            record.Numero,
            record.Resultado
        });
        return filasAfectadas > 0;
    }

    public async Task <NumberRecordGet?> GetByIdAsync(int id)
    {
        const string consulta = @"
            SELECT Id, Numero, Resultado, Fecha
            FROM NumberRecords
            WHERE Id = @Id;
        ";

        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<NumberRecordGet>(consulta, new { Id = id });
    }

    public async Task<IEnumerable<NumberRecordGet>> GetListAsync()
    {
        const string consulta = @"
            SELECT Id, Numero, Resultado, Fecha
            FROM NumberRecords
            ORDER BY Fecha DESC;
        ";

        using var connection = CreateConnection();
        return await connection.QueryAsync<NumberRecordGet>(consulta);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string consulta = @"
            DELETE FROM NumberRecords
            WHERE Id = @Id;
        ";

        using var connection = CreateConnection();
        var filasAfectadas = await connection.ExecuteAsync(consulta, new { Id = id });
        return filasAfectadas > 0;
    }

}
using InternApi.Interface;
using InternApi.Models;
using Microsoft.Data.Sqlite;

namespace InternApi.Repository;

/// <summary>
/// Armazena CEPs consultados em um banco SQLite local.
/// </summary>
public class CepCacheRepository : ICepCacheRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Inicializa o banco local e garante a tabela de cache.
    /// </summary>
    /// <param name="environment">Ambiente da aplicacao para resolver o caminho do banco.</param>
    public CepCacheRepository(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDirectory);

        var databasePath = Path.Combine(dataDirectory, "cep-cache.db");
        _connectionString = $"Data Source={databasePath}";

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS CepCache (
                Cep TEXT PRIMARY KEY,
                Logradouro TEXT NOT NULL,
                Bairro TEXT NOT NULL,
                Cidade TEXT NOT NULL,
                Estado TEXT NOT NULL,
                AtualizadoEm TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    /// <inheritdoc />
    public async Task<CepCache?> BuscarPorCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Cep, Logradouro, Bairro, Cidade, Estado, AtualizadoEm
            FROM CepCache
            WHERE Cep = $cep;
            """;
        command.Parameters.AddWithValue("$cep", cep);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        return new CepCache
        {
            Cep = reader.GetString(0),
            Logradouro = reader.GetString(1),
            Bairro = reader.GetString(2),
            Cidade = reader.GetString(3),
            Estado = reader.GetString(4),
            AtualizadoEm = DateTimeOffset.Parse(reader.GetString(5))
        };
    }

    /// <inheritdoc />
    public async Task SalvarAsync(CepCache cache, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO CepCache (Cep, Logradouro, Bairro, Cidade, Estado, AtualizadoEm)
            VALUES ($cep, $logradouro, $bairro, $cidade, $estado, $atualizadoEm)
            ON CONFLICT(Cep) DO UPDATE SET
                Logradouro = excluded.Logradouro,
                Bairro = excluded.Bairro,
                Cidade = excluded.Cidade,
                Estado = excluded.Estado,
                AtualizadoEm = excluded.AtualizadoEm;
            """;
        command.Parameters.AddWithValue("$cep", cache.Cep);
        command.Parameters.AddWithValue("$logradouro", cache.Logradouro);
        command.Parameters.AddWithValue("$bairro", cache.Bairro);
        command.Parameters.AddWithValue("$cidade", cache.Cidade);
        command.Parameters.AddWithValue("$estado", cache.Estado);
        command.Parameters.AddWithValue("$atualizadoEm", cache.AtualizadoEm.ToString("O"));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}

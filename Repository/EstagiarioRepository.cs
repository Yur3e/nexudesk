using InternApi.Interface;
using InternApi.Models;
using Microsoft.Data.Sqlite;

namespace InternApi.Repository;

/// <summary>
/// Persiste os estagiarios em um banco SQLite local.
/// </summary>
public class EstagiarioRepository : IEstagiarioRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Inicializa o banco local e garante a tabela de estagiarios.
    /// </summary>
    /// <param name="environment">Ambiente da aplicacao para resolver o caminho do banco.</param>
    public EstagiarioRepository(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDirectory);

        var databasePath = Path.Combine(dataDirectory, "estagiarios.db");
        _connectionString = $"Data Source={databasePath}";

        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Estagiarios (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                Email TEXT NOT NULL,
                Departamento TEXT NOT NULL
            );
            """;
        command.ExecuteNonQuery();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<Estagiario> ListarTodos()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Nome, Email, Departamento
            FROM Estagiarios
            ORDER BY Id;
            """;

        using var reader = command.ExecuteReader();
        var estagiarios = new List<Estagiario>();

        while (reader.Read())
        {
            estagiarios.Add(Mapear(reader));
        }

        return estagiarios.AsReadOnly();
    }

    /// <inheritdoc />
    public Estagiario? BuscarPorId(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Nome, Email, Departamento
            FROM Estagiarios
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();

        return reader.Read() ? Mapear(reader) : null;
    }

    /// <inheritdoc />
    public Estagiario Adicionar(Estagiario estagiario)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Estagiarios (Nome, Email, Departamento)
            VALUES ($nome, $email, $departamento)
            RETURNING Id, Nome, Email, Departamento;
            """;
        command.Parameters.AddWithValue("$nome", estagiario.Nome);
        command.Parameters.AddWithValue("$email", estagiario.Email);
        command.Parameters.AddWithValue("$departamento", estagiario.Departamento);

        using var reader = command.ExecuteReader();
        reader.Read();

        return Mapear(reader);
    }

    /// <inheritdoc />
    public bool Atualizar(Estagiario estagiario)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE Estagiarios
            SET Nome = $nome,
                Email = $email,
                Departamento = $departamento
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", estagiario.Id);
        command.Parameters.AddWithValue("$nome", estagiario.Nome);
        command.Parameters.AddWithValue("$email", estagiario.Email);
        command.Parameters.AddWithValue("$departamento", estagiario.Departamento);

        return command.ExecuteNonQuery() > 0;
    }

    /// <inheritdoc />
    public bool Remover(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            """
            DELETE FROM Estagiarios
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static Estagiario Mapear(SqliteDataReader reader)
    {
        return new Estagiario
        {
            Id = reader.GetInt32(0),
            Nome = reader.GetString(1),
            Email = reader.GetString(2),
            Departamento = reader.GetString(3)
        };
    }
}

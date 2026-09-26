using InternApi.Models;

namespace InternApi.Interface;

/// <summary>
/// Define as operacoes de persistencia para estagiarios.
/// </summary>
public interface IEstagiarioRepository
{
    /// <summary>
    /// Lista todos os estagiarios armazenados.
    /// </summary>
    /// <returns>Colecao de estagiarios.</returns>
    IReadOnlyCollection<Estagiario> ListarTodos();

    /// <summary>
    /// Busca um estagiario pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do estagiario.</param>
    /// <returns>Estagiario encontrado ou nulo.</returns>
    Estagiario? BuscarPorId(int id);

    /// <summary>
    /// Adiciona um novo estagiario.
    /// </summary>
    /// <param name="estagiario">Entidade de dominio.</param>
    /// <returns>Entidade persistida com identificador.</returns>
    Estagiario Adicionar(Estagiario estagiario);

    /// <summary>
    /// Atualiza um estagiario existente.
    /// </summary>
    /// <param name="estagiario">Entidade com os novos dados.</param>
    /// <returns>True quando atualizado.</returns>
    bool Atualizar(Estagiario estagiario);

    /// <summary>
    /// Remove um estagiario pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do estagiario.</param>
    /// <returns>True quando removido.</returns>
    bool Remover(int id);
}

using InternApi.DTOs;

namespace InternApi.Interface;

/// <summary>
/// Define as regras de negócio disponíveis para estagiários.
/// </summary>
public interface IEstagiarioService
{
    /// <summary>
    /// Retorna todos os estagiários cadastrados.
    /// </summary>
    /// <returns>Coleção pronta para resposta da API.</returns>
    IEnumerable<EstagiarioResponse> ListarTodos();

    /// <summary>
    /// Retorna os estagiários cadastrados na versão 2.
    /// </summary>
    /// <param name="nome">Filtro opcional por nome.</param>
    /// <returns>Coleção pronta para resposta da API v2.</returns>
    IEnumerable<EstagiarioV2Response> ListarTodosV2(string? nome);

    /// <summary>
    /// Busca um estagiário pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>Estagiário encontrado ou nulo.</returns>
    EstagiarioResponse? BuscarPorId(int id);

    /// <summary>
    /// Busca um estagiário pelo identificador na versão 2.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>Estagiário encontrado ou nulo.</returns>
    EstagiarioV2Response? BuscarPorIdV2(int id);

    /// <summary>
    /// Cria um novo estagiário.
    /// </summary>
    /// <param name="request">Dados de entrada.</param>
    /// <returns>Estagiário criado.</returns>
    EstagiarioResponse Criar(EstagiarioRequest request);

    /// <summary>
    /// Atualiza um estagiário existente.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <param name="request">Novos dados.</param>
    /// <returns>Estagiário atualizado ou nulo quando não encontrado.</returns>
    EstagiarioResponse? Atualizar(int id, EstagiarioRequest request);

    /// <summary>
    /// Remove um estagiário existente.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>True quando removido; false quando não encontrado.</returns>
    bool Remover(int id);
}

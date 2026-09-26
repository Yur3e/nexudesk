using InternApi.Models;

namespace InternApi.Interface;

/// <summary>
/// Define o armazenamento persistente do cache de CEP.
/// </summary>
public interface ICepCacheRepository
{
    /// <summary>
    /// Busca um CEP no cache local.
    /// </summary>
    /// <param name="cep">CEP normalizado com 8 digitos.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Registro encontrado ou nulo.</returns>
    Task<CepCache?> BuscarPorCepAsync(string cep, CancellationToken cancellationToken = default);

    /// <summary>
    /// Salva ou atualiza um CEP no cache local.
    /// </summary>
    /// <param name="cache">Dados retornados pela consulta externa.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    Task SalvarAsync(CepCache cache, CancellationToken cancellationToken = default);
}

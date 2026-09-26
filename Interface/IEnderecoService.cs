using InternApi.DTOs;

namespace InternApi.Interface;

/// <summary>
/// Define a consulta de endereços via API externa.
/// </summary>
public interface IEnderecoService
{
    /// <summary>
    /// Consulta um endereço a partir do CEP.
    /// </summary>
    /// <param name="cep">CEP com ou sem máscara.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Endereço encontrado ou nulo.</returns>
    Task<EnderecoResponse?> BuscarPorCepAsync(string cep, CancellationToken cancellationToken = default);
}

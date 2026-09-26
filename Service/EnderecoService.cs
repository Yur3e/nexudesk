using InternApi.DTOs;
using InternApi.Interface;
using InternApi.Models;

namespace InternApi.Service;

/// <summary>
/// Consome a API do ViaCEP usando IHttpClientFactory.
/// </summary>
public class EnderecoService : IEnderecoService
{
    private readonly ICepCacheRepository _cepCacheRepository;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Inicializa o servico com cache persistente e HttpClient.
    /// </summary>
    /// <param name="cepCacheRepository">Repositorio persistente do cache de CEP.</param>
    /// <param name="httpClientFactory">Fabrica registrada no Program.</param>
    public EnderecoService(ICepCacheRepository cepCacheRepository, IHttpClientFactory httpClientFactory)
    {
        _cepCacheRepository = cepCacheRepository;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Consulta um endereco pelo CEP informado.
    /// </summary>
    /// <param name="cep">CEP com ou sem mascara.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns>Endereco encontrado ou nulo.</returns>
    public async Task<EnderecoResponse?> BuscarPorCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        var cepNormalizado = new string(cep.Where(char.IsDigit).ToArray());
        var cache = await _cepCacheRepository.BuscarPorCepAsync(cepNormalizado, cancellationToken);

        if (cache is not null)
        {
            return new EnderecoResponse
            {
                Logradouro = cache.Logradouro,
                Bairro = cache.Bairro,
                Cidade = cache.Cidade,
                Estado = cache.Estado
            };
        }

        var client = _httpClientFactory.CreateClient("ViaCep");
        using var response = await client.GetAsync($"{cepNormalizado}/json/", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"A consulta ao ViaCEP retornou status {(int)response.StatusCode}.",
                null,
                response.StatusCode);
        }

        var viaCepResponse = await response.Content.ReadFromJsonAsync<ViaCepResponse>(cancellationToken: cancellationToken);

        if (viaCepResponse is null)
        {
            throw new HttpRequestException("A resposta do ViaCEP veio vazia.");
        }

        if (viaCepResponse.Erro)
        {
            return null;
        }

        var endereco = new EnderecoResponse
        {
            Logradouro = viaCepResponse.Logradouro ?? string.Empty,
            Bairro = viaCepResponse.Bairro ?? string.Empty,
            Cidade = viaCepResponse.Cidade ?? string.Empty,
            Estado = viaCepResponse.Estado ?? string.Empty
        };

        await _cepCacheRepository.SalvarAsync(new CepCache
        {
            Cep = cepNormalizado,
            Logradouro = endereco.Logradouro,
            Bairro = endereco.Bairro,
            Cidade = endereco.Cidade,
            Estado = endereco.Estado,
            AtualizadoEm = DateTimeOffset.UtcNow
        }, cancellationToken);

        return endereco;
    }
}

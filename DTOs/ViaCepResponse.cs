using System.Text.Json.Serialization;

namespace InternApi.DTOs;

/// <summary>
/// Mapeia apenas os campos usados da resposta do ViaCEP.
/// </summary>
public class ViaCepResponse
{
    /// <summary>
    /// Logradouro do CEP consultado.
    /// </summary>
    [JsonPropertyName("logradouro")]
    public string? Logradouro { get; set; }

    /// <summary>
    /// Bairro do CEP consultado.
    /// </summary>
    [JsonPropertyName("bairro")]
    public string? Bairro { get; set; }

    /// <summary>
    /// Cidade do CEP consultado.
    /// </summary>
    [JsonPropertyName("localidade")]
    public string? Cidade { get; set; }

    /// <summary>
    /// UF do CEP consultado.
    /// </summary>
    [JsonPropertyName("uf")]
    public string? Estado { get; set; }

    /// <summary>
    /// Indica quando o CEP não foi encontrado.
    /// </summary>
    [JsonPropertyName("erro")]
    public bool Erro { get; set; }
}

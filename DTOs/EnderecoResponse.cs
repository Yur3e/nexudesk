namespace InternApi.DTOs;

/// <summary>
/// Representa o endereço retornado pela API da aplicação.
/// </summary>
public class EnderecoResponse
{
    /// <summary>
    /// Logradouro retornado pelo CEP.
    /// </summary>
    public string Logradouro { get; set; } = string.Empty;

    /// <summary>
    /// Bairro retornado pelo CEP.
    /// </summary>
    public string Bairro { get; set; } = string.Empty;

    /// <summary>
    /// Cidade retornada pelo CEP.
    /// </summary>
    public string Cidade { get; set; } = string.Empty;

    /// <summary>
    /// Estado retornado pelo CEP.
    /// </summary>
    public string Estado { get; set; } = string.Empty;
}

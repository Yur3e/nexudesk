namespace InternApi.Models;

/// <summary>
/// Representa um CEP persistido no cache local.
/// </summary>
public class CepCache
{
    /// <summary>
    /// CEP normalizado com 8 digitos.
    /// </summary>
    public string Cep { get; set; } = string.Empty;

    /// <summary>
    /// Logradouro retornado pela consulta.
    /// </summary>
    public string Logradouro { get; set; } = string.Empty;

    /// <summary>
    /// Bairro retornado pela consulta.
    /// </summary>
    public string Bairro { get; set; } = string.Empty;

    /// <summary>
    /// Cidade retornada pela consulta.
    /// </summary>
    public string Cidade { get; set; } = string.Empty;

    /// <summary>
    /// Estado retornado pela consulta.
    /// </summary>
    public string Estado { get; set; } = string.Empty;

    /// <summary>
    /// Momento da ultima atualizacao do cache.
    /// </summary>
    public DateTimeOffset AtualizadoEm { get; set; }
}

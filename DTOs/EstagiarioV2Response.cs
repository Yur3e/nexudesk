namespace InternApi.DTOs;

/// <summary>
/// Representa os dados devolvidos pela API na versão 2.
/// </summary>
public class EstagiarioV2Response : EstagiarioResponse
{
    /// <summary>
    /// Resumo com nome e departamento.
    /// </summary>
    public string Resumo { get; set; } = string.Empty;
}

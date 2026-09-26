namespace InternApi.Models;

/// <summary>
/// Representa um estagiário exposto pela API.
/// </summary>
public class Estagiario
{
    /// <summary>
    /// Identificador único do estagiário.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome completo do estagiário.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail do estagiário.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Departamento onde o estagiário atua.
    /// </summary>
    public string Departamento { get; set; } = string.Empty;
}

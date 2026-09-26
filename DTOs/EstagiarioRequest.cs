using System.ComponentModel.DataAnnotations;

namespace InternApi.DTOs;

/// <summary>
/// Representa os dados enviados para criar ou atualizar um estagiário.
/// </summary>
public class EstagiarioRequest
{
    /// <summary>
    /// Nome completo do estagiário.
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 120 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Endereço de e-mail corporativo.
    /// </summary>
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Departamento de atuação do estagiário.
    /// </summary>
    [Required(ErrorMessage = "O departamento é obrigatório.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "O departamento deve ter entre 2 e 80 caracteres.")]
    public string Departamento { get; set; } = string.Empty;
}

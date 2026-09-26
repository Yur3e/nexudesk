using System.Text.RegularExpressions;
using InternApi.DTOs;
using InternApi.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InternApi.Controller;

/// <summary>
/// Endpoint de consulta de endereço via ViaCEP.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/endereco")]
public class EnderecoController : ControllerBase
{
    private static readonly Regex CepRegex = new(@"^\d{8}$", RegexOptions.Compiled);
    private readonly IEnderecoService _enderecoService;

    /// <summary>
    /// Inicializa o controller de consulta de endereço.
    /// </summary>
    /// <param name="enderecoService">Serviço que consome a API externa.</param>
    public EnderecoController(IEnderecoService enderecoService)
    {
        _enderecoService = enderecoService;
    }

    /// <summary>
    /// Consulta um endereço a partir de um CEP.
    /// </summary>
    /// <param name="cep">CEP com 8 dígitos, com ou sem máscara.</param>
    /// <param name="cancellationToken">Token de cancelamento da requisição.</param>
    /// <returns>Endereço formatado pela API.</returns>
    [HttpGet("{cep}")]
    [ProducesResponseType(typeof(EnderecoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> BuscarPorCep(string cep, CancellationToken cancellationToken)
    {
        var cepNormalizado = new string(cep.Where(char.IsDigit).ToArray());

        if (!CepRegex.IsMatch(cepNormalizado))
        {
            return BadRequest(new { mensagem = "Informe um CEP válido com 8 dígitos." });
        }

        try
        {
            var endereco = await _enderecoService.BuscarPorCepAsync(cepNormalizado, cancellationToken);

            if (endereco is null)
            {
                return NotFound(new { mensagem = $"O CEP {cepNormalizado} não foi encontrado." });
            }

            return Ok(endereco);
        }
        catch (HttpRequestException)
        {
            return StatusCode(StatusCodes.Status502BadGateway, new
            {
                mensagem = "Não foi possível consultar o ViaCEP no momento."
            });
        }
    }
}

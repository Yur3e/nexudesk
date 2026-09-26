using InternApi.Interface;
using InternApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InternApi.Controller;

/// <summary>
/// Endpoints CRUD de estagiários na versão 1.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/estagiarios")]
public class EstagiariosController : ControllerBase
{
    private readonly IEstagiarioService _estagiarioService;

    /// <summary>
    /// Inicializa o controller com o serviço de estagiários.
    /// </summary>
    /// <param name="estagiarioService">Serviço responsável por fornecer os estagiários.</param>
    public EstagiariosController(IEstagiarioService estagiarioService)
    {
        _estagiarioService = estagiarioService;
    }

    /// <summary>
    /// Lista todos os estagiários cadastrados.
    /// </summary>
    /// <returns>Uma coleção com os estagiários cadastrados.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstagiarioResponse>), StatusCodes.Status200OK)]
    public IActionResult ListarTodos()
    {
        var estagiarios = _estagiarioService.ListarTodos();
        return Ok(estagiarios);
    }

    /// <summary>
    /// Busca um estagiário pelo ID.
    /// </summary>
    /// <param name="id">Identificador único do estagiário.</param>
    /// <returns>Dados do estagiário encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EstagiarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarPorId(int id)
    {
        var estagiario = _estagiarioService.BuscarPorId(id);

        if (estagiario is null)
        {
            return NotFound(new { mensagem = $"Estagiário com id {id} não foi encontrado." });
        }

        return Ok(estagiario);
    }

    /// <summary>
    /// Cadastra um novo estagiário.
    /// </summary>
    /// <param name="request">Dados do estagiário a ser criado.</param>
    /// <returns>Estagiário criado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(EstagiarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Criar([FromBody] EstagiarioRequest request)
    {
        var estagiarioCriado = _estagiarioService.Criar(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = estagiarioCriado.Id }, estagiarioCriado);
    }

    /// <summary>
    /// Atualiza os dados de um estagiário.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <param name="request">Novos dados do estagiário.</param>
    /// <returns>Estagiário atualizado.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EstagiarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Atualizar(int id, [FromBody] EstagiarioRequest request)
    {
        var estagiarioAtualizado = _estagiarioService.Atualizar(id, request);

        if (estagiarioAtualizado is null)
        {
            return NotFound(new { mensagem = $"Estagiário com id {id} não foi encontrado." });
        }

        return Ok(estagiarioAtualizado);
    }

    /// <summary>
    /// Remove um estagiário pelo ID.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>Sem conteúdo quando a exclusão for concluída.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Remover(int id)
    {
        var removido = _estagiarioService.Remover(id);

        if (!removido)
        {
            return NotFound(new { mensagem = $"Estagiário com id {id} não foi encontrado." });
        }

        return NoContent();
    }
}

/// <summary>
/// Endpoints de consulta de estagiários na versão 2.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
[Route("api/v2/estagiarios")]
public class EstagiariosV2Controller : ControllerBase
{
    private readonly IEstagiarioService _estagiarioService;

    /// <summary>
    /// Inicializa o controller da versão 2.
    /// </summary>
    /// <param name="estagiarioService">Serviço responsável pelos estagiários.</param>
    public EstagiariosV2Controller(IEstagiarioService estagiarioService)
    {
        _estagiarioService = estagiarioService;
    }

    /// <summary>
    /// Lista os estagiários com filtro opcional por nome e campo de resumo.
    /// </summary>
    /// <param name="nome">Filtro opcional por nome.</param>
    /// <returns>Coleção de estagiários da versão 2.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstagiarioV2Response>), StatusCodes.Status200OK)]
    public IActionResult ListarTodos([FromQuery] string? nome)
    {
        var estagiarios = _estagiarioService.ListarTodosV2(nome);
        return Ok(estagiarios);
    }

    /// <summary>
    /// Busca um estagiário pelo ID retornando o formato da versão 2.
    /// </summary>
    /// <param name="id">Identificador único do estagiário.</param>
    /// <returns>Dados do estagiário encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EstagiarioV2Response), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BuscarPorId(int id)
    {
        var estagiario = _estagiarioService.BuscarPorIdV2(id);

        if (estagiario is null)
        {
            return NotFound(new { mensagem = $"Estagiário com id {id} não foi encontrado." });
        }

        return Ok(estagiario);
    }
}

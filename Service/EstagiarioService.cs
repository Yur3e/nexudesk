using InternApi.Interface;
using InternApi.DTOs;
using InternApi.Models;

namespace InternApi.Service;

/// <summary>
/// Implementa as regras de negócio para estagiários.
/// </summary>
public class EstagiarioService : IEstagiarioService
{
    private readonly IEstagiarioRepository _estagiarioRepository;

    /// <summary>
    /// Inicializa o serviço com o repositório de estagiários.
    /// </summary>
    /// <param name="estagiarioRepository">Repositório em memória.</param>
    public EstagiarioService(IEstagiarioRepository estagiarioRepository)
    {
        _estagiarioRepository = estagiarioRepository;
    }

    /// <summary>
    /// Retorna todos os estagiários disponíveis.
    /// </summary>
    /// <returns>Coleção pronta para a API.</returns>
    public IEnumerable<EstagiarioResponse> ListarTodos()
    {
        return _estagiarioRepository.ListarTodos().Select(MapearParaResponse);
    }

    /// <summary>
    /// Retorna todos os estagiários disponíveis na versão 2.
    /// </summary>
    /// <param name="nome">Filtro opcional por nome.</param>
    /// <returns>Coleção pronta para a API v2.</returns>
    public IEnumerable<EstagiarioV2Response> ListarTodosV2(string? nome)
    {
        var consulta = _estagiarioRepository.ListarTodos().AsEnumerable();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            consulta = consulta.Where(estagiario =>
                estagiario.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
        }

        return consulta.Select(MapearParaResponseV2);
    }

    /// <summary>
    /// Busca um estagiário pelo identificador.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>Estagiário encontrado ou nulo.</returns>
    public EstagiarioResponse? BuscarPorId(int id)
    {
        var estagiario = _estagiarioRepository.BuscarPorId(id);
        return estagiario is null ? null : MapearParaResponse(estagiario);
    }

    /// <summary>
    /// Busca um estagiário pelo identificador na versão 2.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>Estagiário encontrado ou nulo.</returns>
    public EstagiarioV2Response? BuscarPorIdV2(int id)
    {
        var estagiario = _estagiarioRepository.BuscarPorId(id);
        return estagiario is null ? null : MapearParaResponseV2(estagiario);
    }

    /// <summary>
    /// Cria um novo estagiário.
    /// </summary>
    /// <param name="request">Dados recebidos da API.</param>
    /// <returns>Estagiário criado.</returns>
    public EstagiarioResponse Criar(EstagiarioRequest request)
    {
        var estagiario = new Estagiario
        {
            Nome = request.Nome.Trim(),
            Email = request.Email.Trim(),
            Departamento = request.Departamento.Trim()
        };

        var criado = _estagiarioRepository.Adicionar(estagiario);
        return MapearParaResponse(criado);
    }

    /// <summary>
    /// Atualiza um estagiário existente.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <param name="request">Novos dados enviados pela API.</param>
    /// <returns>Estagiário atualizado ou nulo.</returns>
    public EstagiarioResponse? Atualizar(int id, EstagiarioRequest request)
    {
        var estagiario = _estagiarioRepository.BuscarPorId(id);

        if (estagiario is null)
        {
            return null;
        }

        estagiario.Nome = request.Nome.Trim();
        estagiario.Email = request.Email.Trim();
        estagiario.Departamento = request.Departamento.Trim();

        _estagiarioRepository.Atualizar(estagiario);

        return MapearParaResponse(estagiario);
    }

    /// <summary>
    /// Remove um estagiário existente.
    /// </summary>
    /// <param name="id">Identificador do estagiário.</param>
    /// <returns>True quando removido.</returns>
    public bool Remover(int id)
    {
        return _estagiarioRepository.Remover(id);
    }

    private static EstagiarioResponse MapearParaResponse(Estagiario estagiario)
    {
        return new EstagiarioResponse
        {
            Id = estagiario.Id,
            Nome = estagiario.Nome,
            Email = estagiario.Email,
            Departamento = estagiario.Departamento
        };
    }

    private static EstagiarioV2Response MapearParaResponseV2(Estagiario estagiario)
    {
        return new EstagiarioV2Response
        {
            Id = estagiario.Id,
            Nome = estagiario.Nome,
            Email = estagiario.Email,
            Departamento = estagiario.Departamento,
            Resumo = $"{estagiario.Nome} - {estagiario.Departamento}"
        };
    }
}

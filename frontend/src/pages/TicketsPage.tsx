import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { getApiErrorMessage } from '../services/getApiErrorMessage'
import { ticketService } from '../services/ticketService'
import { priorityLabels, statusLabels, type PagedResponse, type TicketListItem, type TicketPriority, type TicketStatus } from '../types/tickets'

const pageSize = 10

const formatDate = (value: string) => new Intl.DateTimeFormat('pt-BR', {
  dateStyle: 'short',
  timeStyle: 'short',
}).format(new Date(value))

export function TicketsPage() {
  const [status, setStatus] = useState<TicketStatus | ''>('')
  const [priority, setPriority] = useState<TicketPriority | ''>('')
  const [assigned, setAssigned] = useState<'all' | 'true' | 'false'>('all')
  const [page, setPage] = useState(1)
  const [result, setResult] = useState<PagedResponse<TicketListItem> | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    const loadTickets = async () => {
      setIsLoading(true)
      setError(null)

      try {
        const response = await ticketService.getPage({
          status: status || undefined,
          priority: priority || undefined,
          assigned: assigned === 'all' ? undefined : assigned === 'true',
          page,
          pageSize,
        })
        setResult(response)
      } catch (exception) {
        setError(getApiErrorMessage(exception, 'Não foi possível carregar os chamados.'))
      } finally {
        setIsLoading(false)
      }
    }

    void loadTickets()
  }, [assigned, page, priority, status])

  const updateFilter = <T,>(setter: (value: T) => void, value: T) => {
    setter(value)
    setPage(1)
  }

  return (
    <main className="page-shell">
      <section className="page-heading">
        <div>
          <h1>Chamados</h1>
          <p>Consulte e acompanhe todos os chamados disponíveis para você.</p>
        </div>
        <Link className="button-primary" to="/tickets/new">Novo chamado</Link>
      </section>

      <section className="filter-bar" aria-label="Filtros de chamados">
        <select aria-label="Filtrar por situação" value={status} onChange={(event) => updateFilter(setStatus, event.target.value as TicketStatus | '')}>
          <option value="">Todas as situações</option>
          <option value="ABERTO">Aberto</option>
          <option value="EM_PROGRESSO">Em andamento</option>
          <option value="RESOLVIDO">Resolvido</option>
          <option value="FECHADO">Fechado</option>
        </select>

        <select aria-label="Filtrar por prioridade" value={priority} onChange={(event) => updateFilter(setPriority, event.target.value as TicketPriority | '')}>
          <option value="">Todas as prioridades</option>
          <option value="BAIXA">Baixa</option>
          <option value="MÉDIA">Média</option>
          <option value="ALTA">Alta</option>
          <option value="CRÍTICA">Crítica</option>
        </select>

        <select aria-label="Filtrar por responsável" value={assigned} onChange={(event) => updateFilter(setAssigned, event.target.value as 'all' | 'true' | 'false')}>
          <option value="all">Com ou sem responsável</option>
          <option value="true">Com responsável</option>
          <option value="false">Sem responsável</option>
        </select>
      </section>

      {error && <p className="form-error" role="alert">{error}</p>}

      {!error && (
        <section className="tickets-table-wrapper">
          {isLoading && <p className="empty-state">Carregando chamados...</p>}
          {!isLoading && result?.items.length === 0 && <p className="empty-state">Nenhum chamado encontrado.</p>}
          {!isLoading && result && result.items.length > 0 && (
            <table className="tickets-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Título</th>
                  <th>Situação</th>
                  <th>Prioridade</th>
                  <th>Criação</th>
                  <th>Responsável</th>
                </tr>
              </thead>
              <tbody>
                {result.items.map((ticket) => (
                  <tr key={ticket.id}>
                    <td>{ticket.id.slice(0, 8)}</td>
                    <td><Link className="ticket-title" to={`/tickets/${ticket.id}`}>{ticket.title}</Link></td>
                    <td><span className="badge">{statusLabels[ticket.status]}</span></td>
                    <td><span className={`badge ${ticket.priority === 'CRÍTICA' ? 'priority-critical' : ''}`}>{priorityLabels[ticket.priority]}</span></td>
                    <td>{formatDate(ticket.createdAt)}</td>
                    <td>{ticket.assignedAgentName ?? 'Não atribuído'}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </section>
      )}

      {result && result.totalPages > 0 && (
        <nav className="pagination" aria-label="Paginação de chamados">
          <span>{result.totalItems} chamado(s) · Página {result.page} de {result.totalPages}</span>
          <button type="button" disabled={page <= 1 || isLoading} onClick={() => setPage((current) => current - 1)}>Anterior</button>
          <button type="button" disabled={page >= result.totalPages || isLoading} onClick={() => setPage((current) => current + 1)}>Próxima</button>
        </nav>
      )}
    </main>
  )
}

import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { getApiErrorMessage } from '../services/getApiErrorMessage'
import { ticketService } from '../services/ticketService'
import { statusLabels, type TicketListItem, type TicketStatus } from '../types/tickets'

interface DashboardCounts {
  open: number
  inProgress: number
  resolved: number
  closed: number
  critical: number | null
  unassigned: number | null
}

const statuses: Array<{ key: keyof Pick<DashboardCounts, 'open' | 'inProgress' | 'resolved' | 'closed'>; value: TicketStatus; label: string }> = [
  { key: 'open', value: 'ABERTO', label: 'Abertos' },
  { key: 'inProgress', value: 'EM_PROGRESSO', label: 'Em andamento' },
  { key: 'resolved', value: 'RESOLVIDO', label: 'Resolvidos' },
  { key: 'closed', value: 'FECHADO', label: 'Fechados' },
]

export function DashboardPage() {
  const { user } = useAuth()
  const [counts, setCounts] = useState<DashboardCounts | null>(null)
  const [recentTickets, setRecentTickets] = useState<TicketListItem[]>([])
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const loadDashboard = async () => {
      setError(null)

      try {
        const [statusResults, recentTicketsResult] = await Promise.all([
          Promise.all(statuses.map(({ value }) => ticketService.getPage({ status: value, pageSize: 1 }))),
          ticketService.getPage({ pageSize: 5 }),
        ])
        const nextCounts: DashboardCounts = {
          open: statusResults[0].totalItems,
          inProgress: statusResults[1].totalItems,
          resolved: statusResults[2].totalItems,
          closed: statusResults[3].totalItems,
          critical: null,
          unassigned: null,
        }

        if (user?.role === 'AGENTE') {
          const [critical, unassigned] = await Promise.all([
            ticketService.getPage({ priority: 'CRÍTICA', pageSize: 1 }),
            ticketService.getPage({ assigned: false, pageSize: 1 }),
          ])
          nextCounts.critical = critical.totalItems
          nextCounts.unassigned = unassigned.totalItems
        }

        setCounts(nextCounts)
        setRecentTickets(recentTicketsResult.items)
      } catch (exception) {
        setError(getApiErrorMessage(exception, 'Não foi possível carregar o dashboard.'))
      }
    }

    void loadDashboard()
  }, [user?.role])

  return (
    <main className="page-shell">
      <section className="dashboard-hero">
        <div>
          <p className="eyebrow">PAINEL DE ATENDIMENTO</p>
          <h1>Olá, {user?.name?.split(' ')[0]}.</h1>
          <p>{user?.role === 'AGENTE' ? 'Veja o que precisa da atenção da sua equipe hoje.' : 'Acompanhe cada etapa dos seus chamados em um só lugar.'}</p>
        </div>
        <Link className="button-primary button-primary-hero" to="/tickets/new"><span aria-hidden="true">+</span> Abrir chamado</Link>
      </section>

      {error && <p className="form-error" role="alert">{error}</p>}

      {!counts && !error && <p className="loading-copy">Atualizando seu painel...</p>}

      {counts && (
        <>
          <section className="summary-grid" aria-label="Resumo de chamados">
            {statuses.map(({ key, label }) => <article className={`summary-card summary-card-${key}`} key={key}><span>{label}</span><strong>{counts[key]}</strong></article>)}
            {counts.critical !== null && <article className="summary-card summary-card-critical"><span>Críticos</span><strong>{counts.critical}</strong></article>}
            {counts.unassigned !== null && <article className="summary-card"><span>Sem responsável</span><strong>{counts.unassigned}</strong></article>}
          </section>

          <section className="details-card recent-tickets">
            <div className="section-title-row"><div><p className="eyebrow">ATIVIDADE RECENTE</p><h2>Chamados recentes</h2></div><Link to="/tickets">Ver todos <span aria-hidden="true">→</span></Link></div>
            {recentTickets.length === 0 && <p>Nenhum chamado recente.</p>}
            {recentTickets.map((ticket) => (
              <Link key={ticket.id} to={`/tickets/${ticket.id}`}>
                <span className="recent-ticket-title">{ticket.title}<small>#{ticket.id.slice(0, 8)}</small></span>
                <strong className="badge">{statusLabels[ticket.status]}</strong>
              </Link>
            ))}
          </section>
        </>
      )}
    </main>
  )
}

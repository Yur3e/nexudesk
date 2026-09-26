import { useEffect, useState, type FormEvent } from 'react'
import { Link, useParams } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { getApiErrorMessage } from '../services/getApiErrorMessage'
import { commentService } from '../services/commentService'
import { ticketService } from '../services/ticketService'
import { priorityLabels, statusLabels, type TicketComment, type TicketDetails, type TicketPriority, type TicketStatus } from '../types/tickets'

const formatDate = (value: string) => new Intl.DateTimeFormat('pt-BR', {
  dateStyle: 'medium',
  timeStyle: 'short',
}).format(new Date(value))

export function TicketDetailsPage() {
  const { ticketId } = useParams()
  const { user } = useAuth()
  const [ticket, setTicket] = useState<TicketDetails | null>(null)
  const [comments, setComments] = useState<TicketComment[]>([])
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [commentContent, setCommentContent] = useState('')
  const [isEditing, setIsEditing] = useState(false)
  const [isLoading, setIsLoading] = useState(true)
  const [isBusy, setIsBusy] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    const loadTicket = async () => {
      if (!ticketId) {
        return
      }

      setIsLoading(true)
      setError(null)

      try {
        const [ticketResponse, commentResponse] = await Promise.all([
          ticketService.getById(ticketId),
          commentService.getByTicketId(ticketId),
        ])
        setTicket(ticketResponse)
        setTitle(ticketResponse.title)
        setDescription(ticketResponse.description)
        setComments(commentResponse)
      } catch (exception) {
        setError(getApiErrorMessage(exception, 'Não foi possível carregar o chamado.'))
      } finally {
        setIsLoading(false)
      }
    }

    void loadTicket()
  }, [ticketId])

  const updateTicket = async (operation: () => Promise<TicketDetails>) => {
    setIsBusy(true)
    setError(null)

    try {
      setTicket(await operation())
    } catch (exception) {
      setError(getApiErrorMessage(exception, 'Não foi possível atualizar o chamado.'))
    } finally {
      setIsBusy(false)
    }
  }

  const handleEdit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    if (!ticketId) {
      return
    }

    await updateTicket(() => ticketService.update(ticketId, { title, description }))
    setIsEditing(false)
  }

  const handleComment = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    if (!ticketId || !commentContent.trim()) {
      return
    }

    setIsBusy(true)
    setError(null)

    try {
      const comment = await commentService.create(ticketId, commentContent)
      setComments((current) => [...current, comment])
      setCommentContent('')
    } catch (exception) {
      setError(getApiErrorMessage(exception, 'Não foi possível adicionar o comentário.'))
    } finally {
      setIsBusy(false)
    }
  }

  if (isLoading) {
    return <main className="page-shell"><p>Carregando chamado...</p></main>
  }

  if (!ticket) {
    return <main className="page-shell"><p className="form-error">{error ?? 'Chamado não encontrado.'}</p><Link to="/tickets">Voltar para chamados</Link></main>
  }

  const isClosed = ticket.status === 'FECHADO'
  const canEdit = user?.id === ticket.createdByUserId && !isClosed
  const isAgent = user?.role === 'AGENTE' && !isClosed

  return (
    <main className="page-shell ticket-details">
      <section className="details-header">
        <div>
          <Link to="/tickets">← Chamados</Link>
          <h1>{ticket.title}</h1>
        </div>
        <span className="badge">{statusLabels[ticket.status]}</span>
      </section>

      {error && <p className="form-error" role="alert">{error}</p>}

      <section className="details-card">
        {!isEditing && <p>{ticket.description}</p>}

        {isEditing && (
          <form className="form-stack" onSubmit={handleEdit}>
            <label className="field" htmlFor="ticket-title">Título<input id="ticket-title" value={title} minLength={3} maxLength={150} onChange={(event) => setTitle(event.target.value)} required /></label>
            <label className="field" htmlFor="ticket-description">Descrição<textarea id="ticket-description" value={description} minLength={10} maxLength={3000} onChange={(event) => setDescription(event.target.value)} required /></label>
            <button className="button-primary" type="submit" disabled={isBusy}>Salvar alterações</button>
          </form>
        )}

        <div className="details-meta">
          <div><span>Autor</span><strong>{ticket.createdByUserName}</strong></div>
          <div><span>Responsável</span><strong>{ticket.assignedAgentName ?? 'Não atribuído'}</strong></div>
          <div><span>Prioridade</span><strong>{priorityLabels[ticket.priority]}</strong></div>
          <div><span>Criado em</span><strong>{formatDate(ticket.createdAt)}</strong></div>
          {ticket.closedAt && <div><span>Fechado em</span><strong>{formatDate(ticket.closedAt)}</strong></div>}
        </div>

        {canEdit && !isEditing && <button className="button-secondary" type="button" onClick={() => setIsEditing(true)}>Editar conteúdo</button>}
      </section>

      {isAgent && (
        <section className="details-card">
          <h2>Ações de agente</h2>
          <div className="agent-actions">
            <select aria-label="Alterar situação" value={ticket.status} disabled={isBusy} onChange={(event) => void updateTicket(() => ticketService.changeStatus(ticket.id, event.target.value as TicketStatus))}>
              <option value="ABERTO">Aberto</option><option value="EM_PROGRESSO">Em andamento</option><option value="RESOLVIDO">Resolvido</option><option value="FECHADO">Fechado</option>
            </select>
            <select aria-label="Alterar prioridade" value={ticket.priority} disabled={isBusy} onChange={(event) => void updateTicket(() => ticketService.changePriority(ticket.id, event.target.value as TicketPriority))}>
              <option value="BAIXA">Baixa</option><option value="MÉDIA">Média</option><option value="ALTA">Alta</option><option value="CRÍTICA">Crítica</option>
            </select>
            {!ticket.assignedAgentId && <button className="button-warning" type="button" disabled={isBusy} onClick={() => void updateTicket(() => ticketService.assignToCurrentAgent(ticket.id))}>Assumir chamado</button>}
          </div>
        </section>
      )}

      <section className="details-card">
        <h2>Comentários</h2>
        {!isClosed && (
          <form className="form-stack" onSubmit={handleComment}>
            <label className="field" htmlFor="comment-content">Adicionar comentário<textarea id="comment-content" value={commentContent} onChange={(event) => setCommentContent(event.target.value)} required /></label>
            <button className="button-primary" type="submit" disabled={isBusy}>Enviar comentário</button>
          </form>
        )}
        {isClosed && <p>Este chamado foi fechado e não aceita novos comentários.</p>}

        <div className="comment-list">
          {comments.length === 0 && <p>Nenhum comentário ainda.</p>}
          {comments.map((comment) => (
            <article className="comment" key={comment.id}>
              <div className="comment-header"><strong>{comment.userName}</strong><span>{formatDate(comment.createdAt)}</span></div>
              <p>{comment.content}</p>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}

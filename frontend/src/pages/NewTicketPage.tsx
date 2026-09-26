import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import { getApiErrorMessage } from '../services/getApiErrorMessage'
import { ticketService } from '../services/ticketService'

export function NewTicketPage() {
  const navigate = useNavigate()
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setError(null)
    setIsSubmitting(true)

    try {
      const ticket = await ticketService.create({ title, description })
      navigate(`/tickets/${ticket.id}`)
    } catch (exception) {
      setError(getApiErrorMessage(exception, 'Não foi possível criar o chamado.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="page-shell">
      <section className="page-card">
        <h1>Novo chamado</h1>
        <p>Descreva o problema para que a equipe possa ajudar.</p>

        <form className="form-stack" onSubmit={handleSubmit}>
          <label className="field" htmlFor="ticket-title">
            Título
            <input id="ticket-title" value={title} onChange={(event) => setTitle(event.target.value)} minLength={3} maxLength={150} required />
          </label>

          <label className="field" htmlFor="ticket-description">
            Descrição
            <textarea id="ticket-description" value={description} onChange={(event) => setDescription(event.target.value)} minLength={10} maxLength={3000} required />
          </label>

          {error && <p className="form-error" role="alert">{error}</p>}

          <button className="button-primary" type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Criando chamado...' : 'Criar chamado'}
          </button>
        </form>
      </section>
    </main>
  )
}

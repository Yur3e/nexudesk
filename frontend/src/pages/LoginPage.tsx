import { useState, type FormEvent } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { Brand } from '../components/Brand'
import { useAuth } from '../contexts/AuthContext'
import { getApiErrorMessage } from '../services/getApiErrorMessage'

export function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)
  const destination = (location.state as { from?: { pathname?: string } } | null)?.from?.pathname ?? '/dashboard'

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setError(null)
    setIsSubmitting(true)

    try {
      await login({ email, password })
      navigate(destination, { replace: true })
    } catch (exception) {
      setError(getApiErrorMessage(exception, 'Não foi possível entrar. Tente novamente.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="auth-shell">
      <div className="auth-layout">
        <aside className="auth-showcase">
          <Link className="auth-brand" to="/login"><Brand inverted /></Link>
          <div className="auth-showcase-content">
            <span className="auth-kicker">ATENDIMENTO SEM ATRITO</span>
            <h1>Todo pedido merece uma resposta clara.</h1>
            <p>Centralize conversas, acompanhe prioridades e deixe sua equipe sempre no controle.</p>
          </div>
          <div className="auth-quote"><span aria-hidden="true">“</span><p>Mais organização para quem atende. Mais transparência para quem precisa.</p></div>
        </aside>

        <section className="auth-card">
          <div className="auth-mobile-brand"><Brand /></div>
          <p className="eyebrow">BEM-VINDO DE VOLTA</p>
          <h1>Entre na sua conta</h1>
          <p className="auth-intro">Acesse seus chamados e mantenha tudo em movimento.</p>

          <form className="form-stack" onSubmit={handleSubmit}>
            <label className="field" htmlFor="email">
              E-mail
              <input id="email" type="email" autoComplete="email" placeholder="voce@empresa.com" value={email} onChange={(event) => setEmail(event.target.value)} required />
            </label>

            <label className="field" htmlFor="password">
              Senha
              <input id="password" type="password" autoComplete="current-password" placeholder="Sua senha" value={password} onChange={(event) => setPassword(event.target.value)} required />
            </label>

            {error && <p className="form-error" role="alert">{error}</p>}

            <button className="button-primary" type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Entrando...' : 'Entrar na plataforma'}
            </button>
          </form>

          <p className="auth-footer">Ainda não possui uma conta? <Link to="/register">Crie sua conta</Link></p>
        </section>
      </div>
    </main>
  )
}

import { useState, type FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Brand } from '../components/Brand'
import { useAuth } from '../contexts/AuthContext'
import { getApiErrorMessage } from '../services/getApiErrorMessage'

export function RegisterPage() {
  const { register } = useAuth()
  const navigate = useNavigate()
  const [name, setName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [passwordConfirmation, setPasswordConfirmation] = useState('')
  const [error, setError] = useState<string | null>(null)
  const [isSubmitting, setIsSubmitting] = useState(false)

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setError(null)

    if (password !== passwordConfirmation) {
      setError('As senhas não coincidem.')
      return
    }

    setIsSubmitting(true)

    try {
      await register({ name, email, password })
      navigate('/dashboard', { replace: true })
    } catch (exception) {
      setError(getApiErrorMessage(exception, 'Não foi possível criar sua conta. Tente novamente.'))
    } finally {
      setIsSubmitting(false)
    }
  }

  return (
    <main className="auth-shell">
      <div className="auth-layout">
        <aside className="auth-showcase auth-showcase-register">
          <Link className="auth-brand" to="/login"><Brand inverted /></Link>
          <div className="auth-showcase-content">
            <span className="auth-kicker">COMECE EM POUCOS MINUTOS</span>
            <h1>Suporte organizado começa por aqui.</h1>
            <p>Crie sua conta para abrir chamados, acompanhar atualizações e conversar com a equipe responsável.</p>
          </div>
          <ul className="auth-feature-list">
            <li>Visão clara da situação de cada pedido</li>
            <li>Histórico completo de comentários</li>
            <li>Atualizações centralizadas em um só lugar</li>
          </ul>
        </aside>

        <section className="auth-card auth-card-register">
          <div className="auth-mobile-brand"><Brand /></div>
          <p className="eyebrow">NOVA CONTA</p>
          <h1>Crie seu acesso</h1>
          <p className="auth-intro">Você estará pronto para abrir seu primeiro chamado.</p>

          <form className="form-stack" onSubmit={handleSubmit}>
            <label className="field" htmlFor="name">
              Nome
              <input id="name" autoComplete="name" placeholder="Como podemos chamar você?" value={name} onChange={(event) => setName(event.target.value)} required />
            </label>

            <label className="field" htmlFor="email">
              E-mail
              <input id="email" type="email" autoComplete="email" placeholder="voce@empresa.com" value={email} onChange={(event) => setEmail(event.target.value)} required />
            </label>

            <label className="field" htmlFor="password">
              Senha
              <input id="password" type="password" autoComplete="new-password" minLength={6} placeholder="No mínimo 6 caracteres" value={password} onChange={(event) => setPassword(event.target.value)} required />
            </label>

            <label className="field" htmlFor="passwordConfirmation">
              Confirmar senha
              <input id="passwordConfirmation" type="password" autoComplete="new-password" minLength={6} placeholder="Repita sua senha" value={passwordConfirmation} onChange={(event) => setPasswordConfirmation(event.target.value)} required />
            </label>

            {error && <p className="form-error" role="alert">{error}</p>}

            <button className="button-primary" type="submit" disabled={isSubmitting}>
              {isSubmitting ? 'Criando conta...' : 'Criar minha conta'}
            </button>
          </form>

          <p className="auth-footer">Já possui uma conta? <Link to="/login">Entrar</Link></p>
        </section>
      </div>
    </main>
  )
}

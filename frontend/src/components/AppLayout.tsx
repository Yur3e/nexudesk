import { NavLink, Outlet, useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'
import { Brand } from './Brand'

export function AppLayout() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login', { replace: true })
  }

  return (
    <div className="app-layout">
      <header className="app-header">
        <NavLink className="brand" to="/dashboard" aria-label="NexoDesk, ir para o dashboard"><Brand inverted /></NavLink>
        <nav className="app-nav" aria-label="Navegação principal">
          <NavLink to="/dashboard">Visão geral</NavLink>
          <NavLink to="/tickets">Chamados</NavLink>
          <NavLink to="/tickets/new">Novo chamado</NavLink>
        </nav>
        <div className="user-menu">
          <span className="user-avatar" aria-hidden="true">{user?.name.slice(0, 1).toUpperCase()}</span>
          <span className="user-identity"><strong>{user?.name}</strong><small>{user?.role === 'AGENTE' ? 'Agente' : 'Solicitante'}</small></span>
          <button type="button" className="button-secondary" onClick={handleLogout}>Sair</button>
        </div>
      </header>
      <Outlet />
    </div>
  )
}

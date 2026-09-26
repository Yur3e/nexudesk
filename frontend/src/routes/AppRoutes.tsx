import { Navigate, Route, Routes } from 'react-router-dom'
import { DashboardPage } from '../pages/DashboardPage'
import { LoginPage } from '../pages/LoginPage'
import { NewTicketPage } from '../pages/NewTicketPage'
import { RegisterPage } from '../pages/RegisterPage'
import { TicketDetailsPage } from '../pages/TicketDetailsPage'
import { TicketsPage } from '../pages/TicketsPage'
import { AppLayout } from '../components/AppLayout'
import { ProtectedRoute } from './ProtectedRoute'

export function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/tickets" element={<TicketsPage />} />
          <Route path="/tickets/new" element={<NewTicketPage />} />
          <Route path="/tickets/:ticketId" element={<TicketDetailsPage />} />
        </Route>
      </Route>

      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  )
}

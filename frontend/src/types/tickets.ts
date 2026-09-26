export type TicketStatus = 'ABERTO' | 'EM_PROGRESSO' | 'RESOLVIDO' | 'FECHADO'
export type TicketPriority = 'BAIXA' | 'MÉDIA' | 'ALTA' | 'CRÍTICA'

export const statusLabels: Record<TicketStatus, string> = {
  ABERTO: 'Aberto',
  EM_PROGRESSO: 'Em andamento',
  RESOLVIDO: 'Resolvido',
  FECHADO: 'Fechado',
}

export const priorityLabels: Record<TicketPriority, string> = {
  BAIXA: 'Baixa',
  MÉDIA: 'Média',
  ALTA: 'Alta',
  CRÍTICA: 'Crítica',
}

export interface TicketListItem {
  id: string
  title: string
  status: TicketStatus
  priority: TicketPriority
  createdAt: string
  assignedAgentId: string | null
  assignedAgentName: string | null
}

export interface TicketDetails extends TicketListItem {
  description: string
  createdByUserId: string
  createdByUserName: string
  updatedAt: string
  closedAt: string | null
}

export interface TicketListQuery {
  status?: TicketStatus
  priority?: TicketPriority
  assigned?: boolean
  page?: number
  pageSize?: number
}

export interface PagedResponse<T> {
  items: T[]
  page: number
  pageSize: number
  totalItems: number
  totalPages: number
}

export interface CreateTicketRequest {
  title: string
  description: string
}

export interface UpdateTicketRequest extends CreateTicketRequest {}

export interface TicketComment {
  id: string
  content: string
  ticketId: string
  userId: string
  userName: string
  createdAt: string
}

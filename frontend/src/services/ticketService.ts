import type {
  CreateTicketRequest,
  PagedResponse,
  TicketDetails,
  TicketListItem,
  TicketListQuery,
  TicketPriority,
  TicketStatus,
  UpdateTicketRequest,
} from '../types/tickets'
import { api } from './api'

export const ticketService = {
  async getPage(query: TicketListQuery = {}): Promise<PagedResponse<TicketListItem>> {
    const { data } = await api.get<PagedResponse<TicketListItem>>('/tickets', { params: query })
    return data
  },

  async getById(ticketId: string): Promise<TicketDetails> {
    const { data } = await api.get<TicketDetails>(`/tickets/${ticketId}`)
    return data
  },

  async create(request: CreateTicketRequest): Promise<{ id: string }> {
    const { data } = await api.post<{ id: string }>('/tickets', request)
    return data
  },

  async update(ticketId: string, request: UpdateTicketRequest): Promise<TicketDetails> {
    const { data } = await api.put<TicketDetails>(`/tickets/${ticketId}`, request)
    return data
  },

  async changeStatus(ticketId: string, status: TicketStatus): Promise<TicketDetails> {
    const { data } = await api.patch<TicketDetails>(`/tickets/${ticketId}/status`, { status })
    return data
  },

  async changePriority(ticketId: string, priority: TicketPriority): Promise<TicketDetails> {
    const { data } = await api.patch<TicketDetails>(`/tickets/${ticketId}/priority`, { priority })
    return data
  },

  async assignToCurrentAgent(ticketId: string): Promise<TicketDetails> {
    const { data } = await api.patch<TicketDetails>(`/tickets/${ticketId}/assign`)
    return data
  },
}

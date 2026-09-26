import type { TicketComment } from '../types/tickets'
import { api } from './api'

export const commentService = {
  async getByTicketId(ticketId: string): Promise<TicketComment[]> {
    const { data } = await api.get<TicketComment[]>(`/tickets/${ticketId}/comments`)
    return data
  },

  async create(ticketId: string, content: string): Promise<TicketComment> {
    const { data } = await api.post<TicketComment>(`/tickets/${ticketId}/comments`, { content })
    return data
  },
}

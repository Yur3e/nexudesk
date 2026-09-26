import type { AuthResponse, LoginRequest, RegisterRequest } from '../types/auth'
import { api } from './api'

export const authService = {
  async login(request: LoginRequest): Promise<AuthResponse> {
    const { data } = await api.post<AuthResponse>('/auth/login', request)
    return data
  },

  async register(request: RegisterRequest): Promise<AuthResponse> {
    const { data } = await api.post<AuthResponse>('/auth/register', request)
    return data
  },
}

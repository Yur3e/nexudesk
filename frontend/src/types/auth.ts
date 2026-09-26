export type UserRole = 'USUARIO' | 'AGENTE'

export interface AuthResponse {
  userId: string
  name: string
  email: string
  role: UserRole
  accessToken: string
}

export interface AuthUser {
  id: string
  name: string
  email: string
  role: UserRole
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  name: string
  email: string
  password: string
}

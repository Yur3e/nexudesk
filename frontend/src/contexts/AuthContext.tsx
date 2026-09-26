import { createContext, useContext, useMemo, useState, type PropsWithChildren } from 'react'
import { authService } from '../services/authService'
import { authStorage } from '../services/api'
import type { AuthResponse, AuthUser, LoginRequest, RegisterRequest } from '../types/auth'

interface AuthContextValue {
  user: AuthUser | null
  isAuthenticated: boolean
  login: (request: LoginRequest) => Promise<void>
  register: (request: RegisterRequest) => Promise<void>
  logout: () => void
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined)

const getStoredUser = (): AuthUser | null => {
  const value = localStorage.getItem(authStorage.userKey)

  if (!value) {
    return null
  }

  try {
    return JSON.parse(value) as AuthUser
  } catch {
    localStorage.removeItem(authStorage.userKey)
    return null
  }
}

export function AuthProvider({ children }: PropsWithChildren) {
  const [user, setUser] = useState<AuthUser | null>(getStoredUser)

  const persistSession = (response: AuthResponse) => {
    const authenticatedUser: AuthUser = {
      id: response.userId,
      name: response.name,
      email: response.email,
      role: response.role,
    }

    localStorage.setItem(authStorage.accessTokenKey, response.accessToken)
    localStorage.setItem(authStorage.userKey, JSON.stringify(authenticatedUser))
    setUser(authenticatedUser)
  }

  const value = useMemo<AuthContextValue>(() => ({
    user,
    isAuthenticated: user !== null,
    login: async (request) => persistSession(await authService.login(request)),
    register: async (request) => persistSession(await authService.register(request)),
    logout: () => {
      localStorage.removeItem(authStorage.accessTokenKey)
      localStorage.removeItem(authStorage.userKey)
      setUser(null)
    },
  }), [user])

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

export function useAuth() {
  const context = useContext(AuthContext)

  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider.')
  }

  return context
}

import axios from 'axios'

const accessTokenKey = 'helpdesk_access_token'

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:8080/api',
})

api.interceptors.request.use((config) => {
  const token = localStorage.getItem(accessTokenKey)

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

export const authStorage = {
  accessTokenKey,
  userKey: 'helpdesk_user',
}

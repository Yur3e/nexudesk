import axios from 'axios'

interface ApiErrorResponse {
  message?: string
}

export function getApiErrorMessage(error: unknown, fallback: string) {
  if (axios.isAxiosError<ApiErrorResponse>(error)) {
    if (!error.response) {
      return 'Não foi possível conectar à API. Confirme que os contêineres do NexoDesk estão em execução.'
    }

    return error.response?.data.message ?? fallback
  }

  return fallback
}

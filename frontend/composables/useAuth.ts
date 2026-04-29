export const useAuth = () => {
  const token = useState<string | null>('auth_token', () => {
    if (import.meta.client) return localStorage.getItem('admin_token')
    return null
  })

  const isAdmin = computed(() => !!token.value)

  const setToken = (t: string) => {
    token.value = t
    if (import.meta.client) localStorage.setItem('admin_token', t)
  }

  const clearToken = () => {
    token.value = null
    if (import.meta.client) localStorage.removeItem('admin_token')
  }

  const restoreToken = () => {
    if (import.meta.client) {
      const stored = localStorage.getItem('admin_token')
      if (stored) token.value = stored
    }
  }

  return { token, isAdmin, setToken, clearToken, restoreToken }
}

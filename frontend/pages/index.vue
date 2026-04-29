<script setup lang="ts">
interface Event {
  id: number
  name: string
  startDate: string
  endDate: string
  maxParticipants: number
  participantCount: number
}

const config = useRuntimeConfig()
const { restoreToken, token, isAdmin } = useAuth()

const showLogin = ref(false)
const showCreateEvent = ref(false)
const registerTarget = ref<{ id: number; name: string } | null>(null)

const events = ref<Event[]>([])
const loading = ref(true)
const error = ref('')

const fetchEvents = async () => {
  loading.value = true
  error.value = ''
  try {
    events.value = await $fetch<Event[]>(`${config.public.apiBase}/api/events`)
  } catch (err) {
    console.error(err)
    error.value = 'Failed to load events.'
  } finally {
    loading.value = false
  }
}

const deleteEvent = async (id: number) => {
  if (!confirm('Are you sure you want to delete this event?')) return
  try {
    await $fetch(`${config.public.apiBase}/api/events/${id}`, {
      method: 'DELETE',
      headers: { Authorization: `Bearer ${token.value}` }
    })
    fetchEvents()
  } catch (err) {
    console.error(err)
    alert('Failed to delete event.')
  }
}

onMounted(() => {
  restoreToken()
  fetchEvents()
})

type SortKey = 'startDate' | 'endDate' | 'duration' | 'maxParticipants'
const searchQuery = ref('')
const sortKey = ref<SortKey>('startDate')
const sortDir = ref<'asc' | 'desc'>('asc')

const filteredSortedEvents = computed(() => {
  const q = searchQuery.value.trim().toLowerCase()

  const filtered = q
    ? events.value.filter(e => e.name.toLowerCase().includes(q))
    : events.value

  const sorted = [...filtered].sort((a, b) => {
    const aStart = new Date(a.startDate).getTime()
    const bStart = new Date(b.startDate).getTime()
    const aEnd = new Date(a.endDate).getTime()
    const bEnd = new Date(b.endDate).getTime()
    const aDuration = aEnd - aStart
    const bDuration = bEnd - bStart

    let av = 0
    let bv = 0

    switch (sortKey.value) {
      case 'startDate':
        av = aStart; bv = bStart; break
      case 'endDate':
        av = aEnd; bv = bEnd; break
      case 'duration':
        av = aDuration; bv = bDuration; break
      case 'maxParticipants':
        av = a.maxParticipants; bv = b.maxParticipants; break
    }

    return sortDir.value === 'asc' ? av - bv : bv - av
  })

  return sorted
})
</script>

<template>
  <div>
    <NavBar @openLogin="showLogin = true" @openCreateEvent="showCreateEvent = true" />

    <main class="container">
      <h1>Events</h1>

      <div class="controls">
        <input
          v-model="searchQuery"
          class="search"
          type="text"
          placeholder="Search by event name..."
        />

        <div class="sort">
          <label for="sortKey">Sort by</label>
          <select id="sortKey" v-model="sortKey">
            <option value="startDate">Start date</option>
            <option value="endDate">End date</option>
            <option value="duration">Duration</option>
            <option value="maxParticipants">Max capacity</option>
          </select>

          <button class="btn btn-sort" @click="sortDir = sortDir === 'asc' ? 'desc' : 'asc'">
            {{ sortDir === 'asc' ? '↑' : '↓' }}
          </button>
        </div>
      </div>

      <p v-if="error" class="error">{{ error }}</p>

      <div v-if="loading" class="loading">
        <p>Loading events...</p>
      </div>

      <div v-else-if="filteredSortedEvents.length === 0" class="empty">
        <p>No events found.</p>
      </div>

      <div v-else class="events-grid">
        <EventCard
          v-for="event in filteredSortedEvents"
          :key="event.id"
          :event="event"
          :canDelete="isAdmin"
          @register="registerTarget = { id: event.id, name: event.name }"
          @delete="deleteEvent(event.id)"
        />
      </div>
    </main>

    <LoginModal v-if="showLogin" @close="showLogin = false" @login="fetchEvents" />
    <CreateEventModal
      v-if="showCreateEvent"
      @close="showCreateEvent = false"
      @created="fetchEvents"
    />
    <RegisterModal
      v-if="registerTarget"
      :eventId="registerTarget.id"
      :eventName="registerTarget.name"
      @close="registerTarget = null"
      @registered="fetchEvents"
    />
  </div>
</template>

<style scoped>
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

h1 {
  margin: 0 0 2rem 0;
  font-size: 2rem;
  color: #1a1a1a;
}

.error {
  padding: 1rem;
  background: #f8d7da;
  color: #721c24;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.loading,
.empty {
  text-align: center;
  padding: 2rem;
  color: #666;
}

.events-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.controls {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  align-items: center;
  margin-bottom: 1.5rem;
}

.search {
  flex: 1;
  min-width: 240px;
  padding: 0.7rem 0.9rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
}

.sort {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

select {
  padding: 0.6rem 0.8rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background: #fff;
}

.btn-sort {
  background: #fff;
  color: #111827;
  border: 1px solid #d1d5db;
  padding: 0.4rem 0.5rem;
  font-size: 2rem;
  line-height: 1;
  border-radius: 8px;
}
</style>

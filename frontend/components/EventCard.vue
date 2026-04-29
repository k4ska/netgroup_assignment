<script setup lang="ts">
interface Event {
  id: number
  name: string
  startDate: string
  endDate: string
  maxParticipants: number
  participantCount: number
}

defineProps<{ event: Event; canDelete: boolean }>()
defineEmits<{ register: []; delete: [] }>()

const formatDate = (iso: string) =>
  new Date(iso).toLocaleString('en-GB', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
    hour12: false
  })
</script>

<template>
  <div class="card">
    <div class="header">
      <h3>{{ event.name }}</h3>
      <button v-if="canDelete" class="btn btn-danger" @click="$emit('delete')">Delete</button>
    </div>
    <p class="date">
      {{ formatDate(event.startDate) }} - {{ formatDate(event.endDate) }}
    </p>
    <p class="duration">
      Duration: {{ Math.ceil((new Date(event.endDate).getTime() - new Date(event.startDate).getTime()) / (1000 * 60 * 60)) }} hours
    </p>
    <p class="capacity">
      {{ event.participantCount }} / {{ event.maxParticipants }} participants
    </p>
    <button
      class="btn btn-register"
      :disabled="event.participantCount >= event.maxParticipants"
      @click="$emit('register')"
    >
      {{ event.participantCount >= event.maxParticipants ? 'Full' : 'Register' }}
    </button>
  </div>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
}
.card {
  background: white;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  padding: 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  transition: box-shadow 0.2s;
}

.card:hover {
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

h3 {
  margin: 0;
  font-size: 1.2rem;
  color: #1a1a1a;
}

.date {
  margin: 0;
  font-size: 0.9rem;
  color: #666;
}

.duration {
  margin: 0;
  font-size: 0.9rem;
  color: #666;
}

.capacity {
  margin: 0;
  font-size: 0.9rem;
  color: #666;
}

.btn {
  padding: 0.6rem 1.2rem;
  border: none;
  border-radius: 4px;
  font-size: 0.9rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-register {
  background: #28a745;
  color: white;
}

.btn-register:hover:not(:disabled) {
  background: #218838;
}

.btn-register:disabled {
  background: #ccc;
  cursor: not-allowed;
}
.btn-danger {
  background: #dc3545;
  color: white;
}
.btn-danger:hover {
  background: #c82333;
}
</style>

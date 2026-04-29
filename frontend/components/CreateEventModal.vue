<script setup lang="ts">
const config = useRuntimeConfig()
const { token } = useAuth()

const emit = defineEmits<{
  close: []
  created: []
}>()

const name = ref('')
const startDate = ref('')
const endDate = ref('')
const maxParticipants = ref<number | ''>('')
const error = ref('')
const loading = ref(false)

const submit = async () => {
  error.value = ''
  if (!name.value.trim()) { error.value = 'Event name is required'; return }
  if (!startDate.value) { error.value = 'Start date is required'; return }
  if (!endDate.value) { error.value = 'End date is required'; return }
  if (new Date(startDate.value) >= new Date(endDate.value)) {
    error.value = 'Start date must be before end date';
    return;
  }
  if (!maxParticipants.value || maxParticipants.value <= 0) { error.value = 'Capacity must be greater than 0'; return }

  loading.value = true
  try {
    await $fetch(`${config.public.apiBase}/api/events`, {
      method: 'POST',
      headers: { Authorization: `Bearer ${token.value}` },
      body: {
        name: name.value,
        startDate: startDate.value,
        endDate: endDate.value,
        maxParticipants: maxParticipants.value
      }
    })
    emit('created')
    emit('close')
  } catch (err: any) {
    error.value = err.message || 'Failed to create event'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="overlay" @click="emit('close')">
    <div class="modal" @click.stop>
      <h2>Create Event</h2>
      <form @submit.prevent="submit">
        <div class="field">
          <label for="name">Event Name</label>
          <input
            id="name"
            v-model="name"
            type="text"
            placeholder="e.g., Annual Conference"
            :disabled="loading"
          />
        </div>
        <div class="field">
          <label for="startDate">Start Date</label>
          <input
            id="startDate"
            v-model="startDate"
            type="datetime-local"
            lang="en-GB"
            :disabled="loading"
          />
        </div>
        <div class="field">
          <label for="endDate">End Date</label>
          <input
            id="endDate"
            v-model="endDate"
            type="datetime-local"
            lang="en-GB"
            :disabled="loading"
          />
        </div>
        <div class="field">
          <label for="maxParticipants">Max Capacity</label>
          <input
            id="maxParticipants"
            v-model.number="maxParticipants"
            type="number"
            min="1"
            placeholder="100"
            :disabled="loading"
          />
        </div>
        <p v-if="error" class="error">{{ error }}</p>
        <div class="actions">
          <button type="button" class="btn btn-cancel" @click="emit('close')" :disabled="loading">
            Cancel
          </button>
          <button type="submit" class="btn btn-submit" :disabled="loading">
            {{ loading ? 'Creating...' : 'Create Event' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal {
  background: white;
  border-radius: 8px;
  padding: 2rem;
  max-width: 400px;
  width: 90%;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

h2 {
  margin: 0 0 1.5rem 0;
  font-size: 1.5rem;
  color: #1a1a1a;
}

.field {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  font-size: 0.9rem;
  font-weight: 500;
  color: #333;
  margin-bottom: 0.5rem;
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 0.9rem;
  box-sizing: border-box;
}

input:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.1);
}

.error {
  color: #dc3545;
  font-size: 0.85rem;
  margin: 1rem 0;
}

.actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
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

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-cancel {
  background: #f0f0f0;
  color: #333;
}

.btn-cancel:hover:not(:disabled) {
  background: #e0e0e0;
}

.btn-submit {
  background: #007bff;
  color: white;
}

.btn-submit:hover:not(:disabled) {
  background: #0056b3;
}
</style>

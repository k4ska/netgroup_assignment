<script setup lang="ts">
const config = useRuntimeConfig()

const props = defineProps<{
  eventId: number
  eventName: string
}>()

const emit = defineEmits<{
  close: []
  registered: []
}>()

const firstName = ref('')
const lastName = ref('')
const personalIdCode = ref('')
const error = ref('')
const success = ref(false)
const loading = ref(false)

const submit = async () => {
  error.value = ''
  if (!firstName.value.trim() || !lastName.value.trim() || !personalIdCode.value.trim()) {
    error.value = 'Please fill in all the fields'
    return
  }

  if (!/^\d{11}$/.test(personalIdCode.value.trim())) {
    error.value = 'Personal ID Code must be 11 digits'
    return
  }

  loading.value = true
  try {
    await $fetch(`${config.public.apiBase}/api/events/${props.eventId}/register`, {
      method: 'POST',
      body: {
        firstName: firstName.value,
        lastName: lastName.value,
        personalIdCode: personalIdCode.value
      }
    })
    success.value = true
    emit('registered')
  } catch (e: any) {
    error.value = e?.data?.message || 'Registration failed'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="overlay" @click.self="emit('close')">
    <div class="modal">
      <div class="header">
        <h2>Register for Event</h2>
        <p class="event-name">{{ eventName }}</p>
      </div>

      <template v-if="!success">
        <form @submit.prevent="submit">
          <div class="field">
            <label for="firstName">First Name</label>
            <input id="firstName" v-model="firstName" type="text" required placeholder="John" />
          </div>
          <div class="field">
            <label for="lastName">Last Name</label>
            <input id="lastName" v-model="lastName" type="text" required placeholder="Doe" />
          </div>
          <div class="field">
            <label for="personalIdCode">Personal ID Code</label>
            <input
              id="personalIdCode"
              v-model="personalIdCode"
              type="text"
              required
              placeholder="38001010000"
            />
          </div>

          <p v-if="error" class="error">{{ error }}</p>

          <div class="actions">
            <button type="button" class="btn btn-cancel" @click="emit('close')">
              Cancel
            </button>
            <button type="submit" class="btn btn-submit" :disabled="loading">
              {{ loading ? 'Submitting...' : 'Register' }}
            </button>
          </div>
        </form>
      </template>

      <template v-else>
        <div class="success-box">
          <h3>Registration complete</h3>
          <p>Your registration has been submitted.</p>
        </div>
        <div class="actions">
          <button class="btn btn-submit" @click="emit('close')">Close</button>
        </div>
      </template>
    </div>
  </div>
</template>

<style scoped>
.overlay {
  position: fixed;
  inset: 0;
  background: rgba(15, 23, 42, 0.55);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1rem;
  z-index: 1000;
}

.modal {
  width: 100%;
  max-width: 460px;
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 20px 50px rgba(0, 0, 0, 0.12);
}

.header {
  margin-bottom: 1.25rem;
}

h2 {
  margin: 0 0 0.25rem;
  font-size: 1.25rem;
  font-weight: 600;
  color: #111827;
}

.event-name {
  margin: 0;
  font-size: 0.95rem;
  color: #6b7280;
}

.field {
  margin-bottom: 1rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  font-size: 0.875rem;
  font-weight: 500;
  color: #374151;
}

input {
  width: 100%;
  padding: 0.75rem 0.875rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  background: #fff;
  color: #111827;
  font: inherit;
}

input:focus {
  outline: none;
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
}

.error {
  margin: 0.5rem 0 0;
  color: #b91c1c;
  font-size: 0.875rem;
}

.success-box {
  background: #f9fafb;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 1rem;
  text-align: center;
  margin-bottom: 1rem;
}

.success-box h3 {
  margin: 0 0 0.25rem;
  font-size: 1rem;
  color: #111827;
}

.success-box p {
  margin: 0;
  font-size: 0.9rem;
  color: #6b7280;
}

.actions {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  margin-top: 1.25rem;
}

.btn {
  min-width: 96px;
  padding: 0.7rem 1rem;
  border: 0;
  border-radius: 8px;
  font: inherit;
  font-weight: 600;
  cursor: pointer;
}

.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.btn-cancel {
  background: #f3f4f6;
  color: #111827;
}

.btn-cancel:hover:not(:disabled) {
  background: #e5e7eb;
}

.btn-submit {
  background: #2563eb;
  color: #fff;
}

.btn-submit:hover:not(:disabled) {
  background: #1d4ed8;
}
</style>

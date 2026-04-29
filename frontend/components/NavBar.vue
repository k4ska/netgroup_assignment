<script setup lang="ts">
const { isAdmin, clearToken } = useAuth()

const emit = defineEmits<{
  openLogin: []
  openCreateEvent: []
}>()
</script>

<template>
  <nav class="navbar">
    <div class="navbar-container">
      <h1 class="navbar-brand">Event Manager</h1>
      <div class="navbar-actions">
        <button v-if="isAdmin" class="btn btn-primary" @click="emit('openCreateEvent')">
          Create Event
        </button>
        <button v-if="isAdmin" class="btn btn-secondary" @click="clearToken">
          Logout
        </button>
        <button v-else class="btn btn-primary" @click="emit('openLogin')">
          Login
        </button>
      </div>
    </div>
  </nav>
</template>

<style scoped>
.navbar {
  background: #fff;
  border-bottom: 1px solid #e0e0e0;
  padding: 0;
  position: sticky;
  top: 0;
  z-index: 100;
}

.navbar-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.navbar-brand {
  font-size: 1.5rem;
  font-weight: 600;
  color: #1a1a1a;
  margin: 0;
}

.navbar-actions {
  display: flex;
  gap: 1rem;
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

.btn-primary {
  background: #007bff;
  color: white;
}

.btn-primary:hover {
  background: #0056b3;
}

.btn-secondary {
  background: #6c757d;
  color: white;
}

.btn-secondary:hover {
  background: #545b62;
}
</style>

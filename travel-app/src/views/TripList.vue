<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { fetchTrips, createTrip, deleteTrip } from '../api.js'
import TripForm from '../components/TripForm.vue'

const router = useRouter()
const trips = ref([])
const showForm = ref(false)
const loading = ref(true)

async function loadTrips() {
  loading.value = true
  trips.value = await fetchTrips()
  loading.value = false
}

async function handleCreate(trip) {
  await createTrip(trip)
  showForm.value = false
  await loadTrips()
}

async function handleDelete(id, event) {
  event.stopPropagation()
  if (confirm('Supprimer ce voyage ?')) {
    await deleteTrip(id)
    await loadTrips()
  }
}

function goToTrip(id) {
  router.push(`/trip/${id}`)
}

function formatDate(date) {
  if (!date) return ''
  return new Date(date).toLocaleDateString('fr-FR', {
    day: 'numeric',
    month: 'long',
    year: 'numeric'
  })
}

onMounted(loadTrips)
</script>

<template>
  <div>
    <h1>Mes Voyages</h1>

    <button class="btn btn-primary" @click="showForm = true" style="margin-bottom: 1.5rem">
      + Nouveau voyage
    </button>

    <div v-if="loading" class="card">Chargement...</div>

    <div v-else-if="trips.length === 0" class="card empty-state">
      <p>Aucun voyage pour le moment.<br>Créez votre premier voyage !</p>
    </div>

    <div v-else class="trips-grid">
      <div
        v-for="trip in trips"
        :key="trip.id"
        class="card card-clickable"
        @click="goToTrip(trip.id)"
      >
        <h2>{{ trip.title }}</h2>
        <div class="dates" v-if="trip.startDate || trip.endDate">
          {{ formatDate(trip.startDate) }}
          <span v-if="trip.endDate"> → {{ formatDate(trip.endDate) }}</span>
        </div>
        <p class="notes" v-if="trip.notes">{{ trip.notes }}</p>
        <span class="places-count">{{ trip.places?.length || 0 }} lieu(x)</span>
        <div class="actions">
          <button class="btn btn-danger" @click="handleDelete(trip.id, $event)">
            Supprimer
          </button>
        </div>
      </div>
    </div>

    <!-- Modal nouveau voyage -->
    <div v-if="showForm" class="modal-backdrop" @click.self="showForm = false">
      <div class="modal">
        <h2>Nouveau voyage</h2>
        <TripForm @submit="handleCreate" @cancel="showForm = false" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { fetchTrip, updateTrip, addPlace } from '../api.js'
import TripForm from '../components/TripForm.vue'
import PlaceForm from '../components/PlaceForm.vue'
import TripMap from '../components/TripMap.vue'

const props = defineProps(['id'])
const router = useRouter()

const trip = ref(null)
const loading = ref(true)
const showEditForm = ref(false)
const showPlaceForm = ref(false)

async function loadTrip() {
  loading.value = true
  try {
    trip.value = await fetchTrip(props.id)
  } catch {
    router.push('/')
  }
  loading.value = false
}

async function handleUpdate(data) {
  await updateTrip(props.id, data)
  showEditForm.value = false
  await loadTrip()
}

async function handleAddPlace(place) {
  await addPlace(props.id, place)
  showPlaceForm.value = false
  await loadTrip()
}

function formatDate(date) {
  if (!date) return ''
  return new Date(date).toLocaleDateString('fr-FR', {
    day: 'numeric',
    month: 'long',
    year: 'numeric'
  })
}

onMounted(loadTrip)
watch(() => props.id, loadTrip)
</script>

<template>
  <div>
    <router-link to="/" class="back-link">← Retour aux voyages</router-link>

    <div v-if="loading" class="card">Chargement...</div>

    <template v-else-if="trip">
      <div class="card">
        <h1 style="margin-bottom: 0.5rem;">{{ trip.title }}</h1>
        <div class="dates" v-if="trip.startDate || trip.endDate">
          {{ formatDate(trip.startDate) }}
          <span v-if="trip.endDate"> → {{ formatDate(trip.endDate) }}</span>
        </div>
        <p class="notes" v-if="trip.notes" style="margin: 1rem 0; white-space: pre-wrap;">{{ trip.notes }}</p>
        <div class="actions">
          <button class="btn btn-primary" @click="showEditForm = true">Modifier</button>
        </div>
      </div>

      <!-- Carte -->
      <div class="card">
        <h2>Carte</h2>
        <TripMap :places="trip.places" />
      </div>

      <!-- Lieux -->
      <div class="card">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem;">
          <h2>Lieux visités</h2>
          <button class="btn btn-primary" @click="showPlaceForm = true">+ Ajouter</button>
        </div>

        <div v-if="trip.places?.length === 0" class="empty-state">
          <p>Aucun lieu pour ce voyage.<br>Ajoutez votre premier lieu !</p>
        </div>

        <div v-else>
          <div v-for="place in trip.places" :key="place.id" class="place-item">
            <span class="name">{{ place.name }}</span>
            <span class="coords">{{ place.latitude.toFixed(4) }}, {{ place.longitude.toFixed(4) }}</span>
          </div>
        </div>
      </div>
    </template>

    <!-- Modal modifier voyage -->
    <div v-if="showEditForm" class="modal-backdrop" @click.self="showEditForm = false">
      <div class="modal">
        <h2>Modifier le voyage</h2>
        <TripForm :initial="trip" @submit="handleUpdate" @cancel="showEditForm = false" />
      </div>
    </div>

    <!-- Modal ajouter lieu -->
    <div v-if="showPlaceForm" class="modal-backdrop" @click.self="showPlaceForm = false">
      <div class="modal">
        <h2>Ajouter un lieu</h2>
        <PlaceForm @submit="handleAddPlace" @cancel="showPlaceForm = false" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const emit = defineEmits(['submit', 'cancel'])

const form = ref({
  name: '',
  latitude: '',
  longitude: ''
})

function handleSubmit() {
  emit('submit', {
    name: form.value.name,
    latitude: parseFloat(form.value.latitude),
    longitude: parseFloat(form.value.longitude)
  })
}

// Récupère la position actuelle de l'utilisateur
function useMyLocation() {
  if ('geolocation' in navigator) {
    navigator.geolocation.getCurrentPosition(
      (pos) => {
        form.value.latitude = pos.coords.latitude.toFixed(6)
        form.value.longitude = pos.coords.longitude.toFixed(6)
      },
      () => alert('Impossible de récupérer votre position')
    )
  }
}
</script>

<template>
  <form @submit.prevent="handleSubmit">
    <div class="form-group">
      <label>Nom du lieu *</label>
      <input v-model="form.name" type="text" required placeholder="Tour Eiffel" />
    </div>

    <div class="form-group">
      <label>Latitude *</label>
      <input v-model="form.latitude" type="number" step="any" required placeholder="48.8584" />
    </div>

    <div class="form-group">
      <label>Longitude *</label>
      <input v-model="form.longitude" type="number" step="any" required placeholder="2.2945" />
    </div>

    <button type="button" class="btn btn-secondary" @click="useMyLocation" style="margin-bottom: 1rem;">
      Utiliser ma position
    </button>

    <div class="actions">
      <button type="submit" class="btn btn-primary">Ajouter</button>
      <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Annuler</button>
    </div>
  </form>
</template>

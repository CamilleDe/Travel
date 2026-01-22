<script setup>
import { ref, onMounted, watch } from 'vue'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'

const props = defineProps(['places'])
const mapContainer = ref(null)
let map = null
let markers = []

// Fix pour les icônes Leaflet avec Vite
delete L.Icon.Default.prototype._getIconUrl
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
  iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png'
})

function updateMarkers() {
  // Supprime les anciens markers
  markers.forEach(m => map.removeLayer(m))
  markers = []

  if (!props.places?.length) return

  // Ajoute les nouveaux markers
  props.places.forEach(place => {
    const marker = L.marker([place.latitude, place.longitude])
      .addTo(map)
      .bindPopup(`<strong>${place.name}</strong>`)
    markers.push(marker)
  })

  // Centre la carte sur les markers
  if (markers.length > 0) {
    const group = L.featureGroup(markers)
    map.fitBounds(group.getBounds().pad(0.2))
  }
}

onMounted(() => {
  // Initialise la carte centrée sur la France
  map = L.map(mapContainer.value).setView([46.603354, 1.888334], 5)
  
  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors'
  }).addTo(map)

  updateMarkers()
})

watch(() => props.places, updateMarkers, { deep: true })
</script>

<template>
  <div ref="mapContainer" class="map-container"></div>
</template>

<style scoped>
.map-container {
  height: 400px;
  border-radius: 8px;
  z-index: 1;
}
</style>

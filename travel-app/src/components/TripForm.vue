<script setup>
import { ref, onMounted } from 'vue'

const props = defineProps(['initial'])
const emit = defineEmits(['submit', 'cancel'])

const form = ref({
  title: '',
  startDate: '',
  endDate: '',
  notes: ''
})

function handleSubmit() {
  emit('submit', {
    title: form.value.title,
    startDate: form.value.startDate || null,
    endDate: form.value.endDate || null,
    notes: form.value.notes || null
  })
}

onMounted(() => {
  if (props.initial) {
    form.value = {
      title: props.initial.title || '',
      startDate: props.initial.startDate || '',
      endDate: props.initial.endDate || '',
      notes: props.initial.notes || ''
    }
  }
})
</script>

<template>
  <form @submit.prevent="handleSubmit">
    <div class="form-group">
      <label>Titre *</label>
      <input v-model="form.title" type="text" required placeholder="Mon super voyage" />
    </div>

    <div class="form-group">
      <label>Date de début</label>
      <input v-model="form.startDate" type="date" />
    </div>

    <div class="form-group">
      <label>Date de fin</label>
      <input v-model="form.endDate" type="date" />
    </div>

    <div class="form-group">
      <label>Notes</label>
      <textarea v-model="form.notes" placeholder="Mes souvenirs, mes impressions..."></textarea>
    </div>

    <div class="actions">
      <button type="submit" class="btn btn-primary">Enregistrer</button>
      <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Annuler</button>
    </div>
  </form>
</template>

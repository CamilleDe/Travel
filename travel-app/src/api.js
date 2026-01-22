// Configuration API simple
const API_BASE = 'http://localhost:5062'

export async function fetchTrips() {
  const res = await fetch(`${API_BASE}/trips`)
  return res.json()
}

export async function fetchTrip(id) {
  const res = await fetch(`${API_BASE}/trips/${id}`)
  if (!res.ok) throw new Error('Trip not found')
  return res.json()
}

export async function createTrip(trip) {
  const res = await fetch(`${API_BASE}/trips`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(trip)
  })
  return res.json()
}

export async function updateTrip(id, trip) {
  await fetch(`${API_BASE}/trips/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(trip)
  })
}

export async function deleteTrip(id) {
  await fetch(`${API_BASE}/trips/${id}`, { method: 'DELETE' })
}

export async function addPlace(tripId, place) {
  const res = await fetch(`${API_BASE}/trips/${tripId}/places`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(place)
  })
  return res.json()
}

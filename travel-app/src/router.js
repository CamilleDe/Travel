import { createRouter, createWebHistory } from 'vue-router'
import TripList from './views/TripList.vue'
import TripDetail from './views/TripDetail.vue'

const routes = [
  { path: '/', component: TripList },
  { path: '/trip/:id', component: TripDetail, props: true }
]

export default createRouter({
  history: createWebHistory(),
  routes
})

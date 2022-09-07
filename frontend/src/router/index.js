import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import GamesView from '../views/GamesView.vue'
import AdminCalendar from '../views/AdminCalendar.vue'
import ReservationsListAdmin from '../views/ReservationsListAdmin.vue'
import ScheduleView from '../views/ScheduleView.vue'
import BookingsCalendar from '../views/MyBookings.vue';
import PageError from '../views/PageError.vue'

const routes = [
  {
    path: '/',
    name: 'login',
    component: LoginView
  },
  {
    path: '/PageError',
    name: 'PageError',
    component: PageError
  },
  {

    path: '/gamesview',
    name: 'gamesview',
    component: GamesView,
    beforeEnter: (to, from, next) => {
      if (localStorage.PersonRole == 0) {
        next({ name: 'PageError', params: { therror: 'GamesView' }, replace: true });
        console.log("therror", this.therror)
      } else {
        next();
      }
    }
  },
  {
    path: '/reservationslistadmin',
    name: 'reservationlistadmin',
    component: ReservationsListAdmin,
    beforeEnter: (to, from, next) => {
      if (localStorage.PersonRole == 0) {
        next({ name: 'PageError', params: { therror: 'ReservationsListAdmin' }, replace: true });
        console.log("therror", this.therror)
      } else {
        next();
      }
    }
  },
  {
    path: '/admincalendar',
    name: 'admincalendar',
    component: AdminCalendar,
    beforeEnter: (to, from, next) => {
      if (localStorage.PersonRole == 0) {
        next({ name: 'PageError', params: { therror: 'AdminCalendar' }, replace: true });
        console.log("therror", this.therror)
      } else {
        next();
      }
    }
  },
  {
    path: '/schedule',
    name: 'schedule',
    component: ScheduleView
  },
  {
    path: '/mybookings',
    name: 'mybookings',
    component: BookingsCalendar
  }

]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router

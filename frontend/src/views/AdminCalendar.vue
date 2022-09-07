<template>
<!--Cette page permet de gérer les différents événements du calendrier (utilisé par l'admin et le super admin pour ajouter,
  modifier,deplacer,supprimer,resize les événements)
-->
  <NavBar />
  <CalendarVue />
</template>
<script>
import NavBar from './NavBar.vue'
import CalendarVue from '../components/CalendarVue.vue'

export default {
  components: { NavBar, CalendarVue },
}
</script>

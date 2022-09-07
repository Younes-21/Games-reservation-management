<template>
<!--Cette page représente l'emploi du temps -->
  <NavBar />
  <WeekProgram />

</template>
<script>
import NavBar from './NavBar.vue'
import WeekProgram from '../components/WeekProgram.vue';

export default {
  components: { NavBar, WeekProgram },
}
</script>

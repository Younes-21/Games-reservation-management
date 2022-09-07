<template>

  <NavBar />

  <hr />
  <BookingsCalendar />

</template>
<script>
import NavBar from './NavBar.vue'
import BookingsCalendar from '@/components/BookingsCalendar.vue';

export default {
  components: { NavBar, BookingsCalendar },
}
</script>

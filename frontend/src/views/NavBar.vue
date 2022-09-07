<template>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-F3w7mX95PdgyTmZZMECAngseQB83DfGTowi0iMjiWaeVhAn4FJkqJByhZMI3AhiU" crossorigin="anonymous">
  <!-- Navbar -->
  <nav class="navbar navbar-expand-lg navbar-light bg-light ">
    <!-- Container wrapper -->
    <div class="container-fluid">
      <!-- Toggle button -->
      <button class="navbar-toggler" type="button" data-mdb-toggle="collapse" data-mdb-target="#navbarSupportedContent"
        aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <i class="fas fa-bars"></i>
      </button>

      <!-- Collapsible wrapper -->
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <!-- Navbar brand -->
        <a class="navbar-brand mt-2 mt-lg-0" href="#">
          <img src="../assets/webhelp.jpg" height="35" alt="WebHelp Logo" loading="lazy" />
        </a>
        <!-- Left links -->
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link v-if="PersonRole == 2 || PersonRole == 1" class="nav-link" :to="{ name: 'admincalendar' }"><i
                class="fa-regular fa-calendar-plus"></i> Gérer le calendrier</router-link>
          </li>
          <li class="nav-item">
            <router-link v-if="PersonRole == 2 || PersonRole == 1" class="nav-link" :to="{ name: 'gamesview' }"><i
                class="fa-solid fa-dice-d20"></i>Liste des jeux</router-link>
          </li>
          <li class="nav-item">
            <router-link v-if="PersonRole == 2 || PersonRole == 1 || PersonRole == 0" class="nav-link"
              :to="{ name: 'schedule' }"><i class="fa-solid fa-calendar-week"></i>Emploi du temps</router-link>
          </li>
          <li class="nav-item">
            <router-link v-if="PersonRole == 2 || PersonRole == 1 || PersonRole == 0" class="nav-link"
              :to="{ name: 'mybookings' }"><i class="fa-solid fa-arrow-down-up-across-line"></i> Mes réservations
            </router-link>
          </li>
          <li class="nav-item">
            <router-link v-if="PersonRole == 2 || PersonRole == 1" class="nav-link" :to="{ name: 'reservationlistadmin' }"><i
                class="fa-solid fa-arrow-down-up-across-line"></i>Liste des réservations</router-link>
          </li>
          <li class="nav-item" v-if="PersonRole == 2">
            <div class="collapse navbar-collapse" id="navbarNavDarkDropdown">
              <ul class="navbar-nav">
                <li class="nav-item dropdown">
                  <a class="nav-link dropdown-toggle" href="#" id="navbarDarkDropdownMenuLink" role="button"
                    data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="fa-solid fa-location-dot"></i> Site
                  </a>
                  <ul class="dropdown-menu dropdown-menu-dark" aria-labelledby="navbarDarkDropdownMenuLink">
                    <li v-for="site in this.sites" :key="site.id"><a class="dropdown-item"
                        @click="changeSite(site.SiteID)">{{  site.SiteID  }}</a></li>
                  </ul>
                </li>
              </ul>
            </div>
          </li>

        </ul>
        <!-- Left links -->
      </div>
      <!-- Collapsible wrapper -->

      <!-- Right elements -->
      <div class="d-flex align-items-center">
        <!-- Icon -->

        <!-- Notifications -->

        <!-- Avatar -->
        <div class="dropdown">

          <div class="nav-item" style="color:#1D4851;" @click="logout">
            <router-link class="nav-link" :to="{ name: 'login' }" data-bs-toggle="tooltip" data-bs-placement="bottom"
             style="color:rgb(13,85,107)" title="Logout"><i class="fa-solid fa-user-tie"></i>{{  getPersonName() }}&nbsp;{{ this.SiteID }}&nbsp;<i
                class="fa-solid fa-right-from-bracket"></i> </router-link>

          </div>
        </div>
      </div>
      <!-- Right elements -->
    </div>
    <!-- Container wrapper -->
  </nav>
  <br>
  <br>

  <!-- Navbar -->
</template>

<script >
export default {
  data() {
    return {
      PersonID: localStorage.PersonID,
      PersonRole: localStorage.PersonRole,
      SiteID: localStorage.SiteID,
      // PersonName: null,
      persons: [],
      sites: [],
      site: {},
    }
  },

  methods: {
    logout() {
      this.PersonID = null;
      this.PersonRole = null;
      this.SiteID = null;
      localStorage.removeItem('PersonID');
      localStorage.removeItem('PersonRole');
      localStorage.removeItem('SiteID');

      window.localStorage.clear();

      //localStorage.clear();
      //window.location.reload(false); 
      console.log("destroy", localStorage)
    },
    getPersonName() {
      let PersonName = "NAN"
      for (let i = 0; i < this.persons.length; i++) {
        //console.log("here")
        if (this.persons[i].PersonID == this.PersonID) {
          //  console.log("here1")
          console.log(this.persons[i].FirstName)
          PersonName = this.persons[i].FirstName + ' ' + this.persons[i].LastName;
        }
        //console.log("his name is",PersonName)

      }
      // console.log("le nom est :", PersonName)
      return PersonName;
    },
    async getPersons() {
      var response = await fetch('http://localhost:61917/api/person');
      this.persons = await response.json();

    },

    async getSites() {
      var response = await fetch('http://localhost:61917/api/site');
      this.sites = await response.json();

    },
    changeSite(SiteID) {
      console.log("heer", SiteID)

      localStorage.setItem("SiteID", SiteID)

      console.log("localStorage", localStorage)
      window.location.reload(false);

    }
  },

  created() {
    this.getPersons()
    this.getSites()

  }

}
</script>

<style scoped>
.navbar {
  position: fixed;
  width: 100%;
  height: 50px;
  z-index: 1000000;
  top: 0;
}

.dropdown-menu {
  max-height: 200px;
  overflow-y: auto;
}
</style>
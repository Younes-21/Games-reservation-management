<template>

  <NavBar />



  <div class="card" style="background-color: white;">
    <center>
      <div>
        <h3><b style="color:black">Liste des jeux</b></h3>
        <hr />
        <button @click="toggleModal"
          style="background-color: #1D4851;border: #00608d;border-radius: 5px;height: 32px; color: white;">Ajouter un
          jeu</button>
        <hr />
      </div>
    </center>
    <div v-if="showcreategame">
      <CreateGame v-bind:showcreategame="showcreategame" v-bind:toggleModal="toggleModal" />
    </div>
    <div v-if="showupdategame">
      <EditGame v-bind:showupdategame="showupdategame" v-bind:gametoedit="gametoedit"
        v-bind:toggleModalUpdate="toggleModalUpdate" />
    </div>
    <table class="table">
      <thead class="table-dark">

      </thead>
      <tbody>

        <tr style="background-color: #1D4851; color:white;">
          <td border="1px">Nom du jeu</td>
          <td>Site</td>
          <td>Ajouté par</td>
          <td>Date d'ajout</td>
          <td>Status</td>

          <td> &nbsp; &nbsp;Actions</td>
        </tr>

        <tr v-for="game in games" :key="game.GameID">

          <td>{{ game.GameName }}</td>
          <td>{{ findSite(game) }}</td>
          <td>{{ findPerson(game) }}</td>
          <td>{{ game.GameAddDate }}</td>
          <td>
            <template v-if="game.Status == 1">Disponible &nbsp;&nbsp;<i class="fa-solid fa-circle-check"
                style="color:green;"></i></template>
            <template v-else>Indisponible <i class="fa-solid fa-circle-xmark" style="color:red;"></i></template>
          </td>
          <td>
            <template v-if="game.Status == 1"> <a class="btn" @click="getoneGame(game)"><i
                  class="fa-solid fa-pen"></i></a>
              <a class="btn" @click="Disable(game.GameID)"> <i class="fa-solid fa-circle-xmark"></i></a> </template>
            <template v-else>
              <a class="btn" @click="getoneGame(game)"><i class="fa-solid fa-pen"></i></a>
              <a class="btn" @click="Unable(game.GameID)"> <i class="fa-solid fa-circle-check"></i></a>
            </template>
          </td>
        </tr>




      </tbody>
    </table>
  </div>



</template>
<script>
import NavBar from './NavBar.vue'
import CreateGame from '@/components/Modals/CreateGame.vue';
import EditGame from '@/components/Modals/EditGame.vue'
import moment from 'moment';


export default {
  components: { NavBar, CreateGame, EditGame },
  data() {
    return {
      numberbookings: [],
      //  IdSite : localStorage.IdSite,
      gamestofind: [],
      Now: moment().format(),
      scheduule: {},
      schedules: [],
      PersonRole: localStorage.PersonRole,
      SiteID: localStorage.SiteID,
      showcreategame: false,
      showupdategame: false,
      games: [],
      sites: [],
      site: {},
      game: {},
      gametoedit: {},
      persons: []
    }
  },
  methods: {
    toggleModal: function () {
      this.showcreategame = !this.showcreategame;
    },
    toggleModalUpdate: function () {
      this.showupdategame = !this.showupdategame;
    },

    async getoneGame(game) {
      var scheduleid = null;
      // console.log(id)
      for (let i = 0; i < this.schedules.length; i++) {
        if (this.schedules[i].GameID == game.GameID && this.schedules[i].BeginDate > this.Now) {
          scheduleid = this.schedules[i].ScheduleID;
        }
      }
      console.log("scheduleid", scheduleid);
      if (scheduleid != null) {
        var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + scheduleid);
        this.numberbookings = await resp.json();
        if (this.numberbookings[0].Column1 == 0) {
          var response = await fetch("http://localhost:61917/api/game/" + game.GameID);
          this.gametoedit = await response.json();
          this.toggleModalUpdate();
        }
        else {
          alert("Ce jeu ne peut pas être modifié puisqu'il est dèja réservé");
          window.location.reload(false);
        }
      }
      else {
        var res = await fetch("http://localhost:61917/api/game/" + game.GameID);
        this.gametoedit = await res.json();
        this.toggleModalUpdate();
      }

      console.log("scheedule", this.scheduule)

    },
    async getGames() {
      var response = await fetch('http://localhost:61917/api/game/bysite/' + this.SiteID);
      this.games = await response.json();
      //console.log("here",this.games)
    },
    async getSites() {
      var response = await fetch('http://localhost:61917/api/site');
      this.sites = await response.json();
    },
    async getPersons() {
      var response = await fetch('http://localhost:61917/api/person');
      this.persons = await response.json();
    },
    async getSchedules() {
      var response = await fetch('http://localhost:61917/api/schedule/schedulebysite/' + this.SiteID);
      this.schedules = await response.json();
    },
    findSite(game) {
      let site = "NAN";
      for (let i = 0; i < this.sites.length; i++) {
        if (game.SiteID == this.sites[i].SiteID) {
          site = this.sites[i].SiteName;
        }
      }
      return site;
    },
    findPerson(game) {
      let person = "NONE";
      for (let i = 0; i < this.persons.length; i++) {
        if (game.PersonID == this.persons[i].PersonID) {
          person = this.persons[i].FirstName + ' ' + this.persons[i].LastName;
        }
      }
      return person;
    },
    async Unable(id) {
      //console.log(id)
      var response = await fetch("http://localhost:61917/api/game/unable/" + id, {
        method: 'put',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.game)
      });
      // console.log("here");
      this.games.push(await response.json());
      //console.log(this.games)
      window.location.reload(false);
    },
    async Disable(id) {
      var scheduleid = null;
      // console.log(id)
      for (let i = 0; i < this.schedules.length; i++) {
        if (this.schedules[i].GameID == id && this.schedules[i].BeginDate > this.Now) {
          scheduleid = this.schedules[i].ScheduleID;
        }
      }
      console.log("scheduleid", scheduleid);
      if (scheduleid != null) {
        var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + scheduleid);
        this.numberbookings = await resp.json();
        if (this.numberbookings[0].Column1 == 0) {
          var response = await fetch("http://localhost:61917/api/game/disable/" + id, {
            method: 'put',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.game)
          });
          // console.log("here");
          this.games.push(await response.json());
          //console.log(this.games)
          window.location.reload(false);
          //Pour le supprimer
          console.log("suppressionnn")
          var res = await fetch("http://localhost:61917/api/schedule/delete/" + scheduleid, {
            method: 'delete',
            headers: {
              'Content-Type': 'application/json'
            },
            body: JSON.stringify(this.schedule)
          });
          // console.log("here");
          this.schedules.push(await res.json());
          //console.log(this.games)
          window.location.reload(false);
        }
        else {
          alert("Ce jeu ne peut pas être désactivé puisqu'il est dèja réservé");
          window.location.reload(false);
        }
      }
      else {
        var re = await fetch("http://localhost:61917/api/game/disable/" + id, {
          method: 'put',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(this.game)
        });

        this.games.push(await re.json());

        window.location.reload(false);
      }

      console.log("scheedule", this.scheduule)

    },


  },

  created() {
    if (localStorage.length == 0) { this.$router.push({ name: 'login' }) }
    this.getPersons();
    this.getSites();
    this.getGames();
    this.getSchedules();
    console.log("ls", localStorage)
  }

}
</script>

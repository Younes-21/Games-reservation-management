<template>
  <NavBar />
  <div style="background-color:#1D4851 ;">
    <div class="container py-5 text-center">

      <form @submit.prevent="Search">
        <h3 style="color:white">Liste des reservations</h3>
        <h5 style="color:white">Trier par :</h5>
        <br />
        <div class="row" style="color:white">

          <div class="col">
            <select class="form-control" v-model="choosengame.GameName">
              <option selected>Chosissez un jeu...</option>
              <option v-for="game in sitegames" :value="game.GameName" :key="game.GameID">{{  game.GameName  }}</option>
            </select>
          </div>

          <div class="col">
            <input type="date" class="form-control" v-model="choosendate.date">
          </div>
          <div class="col">
            <div class="row">
              <div class="col">
                <button type="submit" class="form-control"><i class="fa-solid fa-magnifying-glass"></i> Search</button>
              </div>
              <!--  <div class="col" >
                  <button type="button" class="form-control" ><i class="fa-solid fa-file-excel"></i>  Download</button>
                </div>-->
            </div>
          </div>
        </div>
      </form>



    </div>
  </div>


  <table class="table">
    <thead class="table-dark">
    </thead>
    <tbody>
      <tr style="background-color: #1D4851; color:white;">
        <td border="1px">Réserver par</td>
        <td>Réserver le</td>
        <td>Jeu</td>
        <td>Nombre de places réservé</td>
        <td>Date début</td>
        <td>Date fin</td>
      </tr>
      <template v-if="this.find == false">
        <tr v-for="booking in bookings" :key="booking.BookingID">
          <template v-if="this.today === findDate(booking)">
            <td>{{ findPerson(booking) }}</td>
            <td>{{ booking.BookingDate }}</td>
            <td>{{ findGame(booking) }}</td>
            <td>
              &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              {{ Booked(booking) }}/{{ AvailableSeats(booking) }}</td>
            <td>{{ findDateBegin(booking) }}</td>
            <td>{{ findDateEnd(booking) }}</td>
          </template>
        </tr>
      </template>
      <template v-if="this.find == true">
        <tr v-for="booking in searchedbookings" :key="booking.BookingID">
          <td>{{ findPerson(booking) }}</td>
          <td>{{ booking.BookingDate }}</td>
          <td>{{ findGame(booking) }}</td>
          <td>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            {{ Booked(booking) }}/{{ AvailableSeats(booking) }}</td>
          <td>{{ findDateBegin(booking) }}</td>
          <td>{{ findDateEnd(booking) }}</td>

        </tr>
      </template>


    </tbody>
  </table>




</template>
<script>
import NavBar from './NavBar.vue'
import moment from 'moment'
export default {
  data() {
    return {
      searchedbookings: [],
      choosendate: {},
      find: false,
      choosengame: {},
      sitegames: [],
      numberbookings: [],
      today: moment().format('DMMyyyy'),
      schedules: [],
      games: [],
      persons: [],
      sites: [],
      PersonRole: localStorage.PersonRole,
      SiteID: localStorage.SiteID,

      bookings: [],
    }
  },
  components: { NavBar },
  methods: {
    findDateBegin(booking) {
      var DateBegin = ""
      for (let i = 0; i < this.schedules.length; i++) {
        if (booking.ScheduleID == this.schedules[i].ScheduleID) {
          DateBegin = this.schedules[i].BeginDate;
        }
      }
      // console.log(moment(DateBegin).format('DMMyyyyTHHmm'))
      return DateBegin;
    },
    AvailableSeats(booking) {
      var availableseats = ""
      for (let i = 0; i < this.schedules.length; i++) {
        if (booking.ScheduleID == this.schedules[i].ScheduleID) {
          availableseats = this.schedules[i].Availableseats;
        }
      }
      // console.log(moment(DateBegin).format('DMMyyyyTHHmm'))
      return availableseats;
    },
    Booked(booking) {
      var nb=0;
      for(let i=0;i<this.bookings.length;i++){
             if(this.bookings[i].ScheduleID == booking.ScheduleID){
              nb++;
             }
      }
      return nb;
    },
    
    findDate(booking) {
      var DateBegin = ""
      for (let i = 0; i < this.schedules.length; i++) {
        if (booking.ScheduleID == this.schedules[i].ScheduleID) {
          DateBegin = this.schedules[i].BeginDate;
        }
      }
      //console.log(moment(DateBegin).format('DMMyyyy'))
      return moment(DateBegin).format('DMMyyyy');
    },

    findDateEnd(booking) {
      var DateEnd = ""
      for (let i = 0; i < this.schedules.length; i++) {
        if (booking.ScheduleID == this.schedules[i].ScheduleID) {
          DateEnd = this.schedules[i].EndDate;
        }
      }
      return DateEnd;
    },
    findGame(booking) {
      var GameName = "NAN"
      var GameID = ""
      for (let i = 0; i < this.schedules.length; i++) {
        if (booking.ScheduleID == this.schedules[i].ScheduleID) {
          GameID = this.schedules[i].GameID;
          for (let j = 0; j < this.games.length; j++) {
            if (this.games[j].GameID == GameID) {
              GameName = this.games[j].GameName
            }
          }
        }
      }
      return GameName;
    },
    findPerson(booking) {
      let person = "NONE";
      for (let i = 0; i < this.persons.length; i++) {
        if (booking.PersonID == this.persons[i].PersonID) {
          person = this.persons[i].FirstName + ' ' + this.persons[i].LastName;
        }
      }
      return person;
    },
    async getPersons() {
      var response = await fetch('http://localhost:61917/api/person');
      this.persons = await response.json();
    },

    async getBookings() {
      var response = await fetch('http://localhost:61917/api/booking/bookingbysite/' + this.SiteID);
      this.bookings = await response.json();
    },
    async getSites() {
      var response = await fetch('http://localhost:61917/api/site');
      this.sites = await response.json();
    },
    async getGames() {
      var response = await fetch('http://localhost:61917/api/game');
      this.games = await response.json();
    },
    async getSiteGames() {
      var response = await fetch('http://localhost:61917/api/game/bysite/' + this.SiteID);
      this.sitegames = await response.json();
    },
    async getSchedules() {
      var response = await fetch('http://localhost:61917/api/schedule');
      this.schedules = await response.json();
    },
    async getBookingsbyname(name,site) {
      var response = await fetch('http://localhost:61917/api/booking/findbyname/' + name+'/'+site);
      this.searchedbookings = await response.json();
      console.log("byname", this.searchedbookings)
    },
    async getBookingsbydate(date,site) {
      var response = await fetch('http://localhost:61917/api/booking/findbydate/' + date+'/'+site);
      this.searchedbookings = await response.json();
      console.log("bydate", this.searchedbookings)
    },
    async getBookingsbydateandname(date, name,site) {
      var response = await fetch('http://localhost:61917/api/booking/findbydateandname/' + date + '/' + name+'/'+site);
      this.searchedbookings = await response.json();
      console.log("dateandname", this.searchedbookings)
    },
    Search() {
      this.find = true;
      console.log("haa hoa",this.choosengame.GameName)
      if (this.choosendate.date != null && this.choosengame.GameName != null) {
       // this.searchedbookings = null;
        console.log("les deux")
        this.getBookingsbydateandname(moment(this.choosendate.date).format('DD-MM-yyyy'), this.choosengame.GameName,this.SiteID)
      }
      else if (this.choosengame.GameName != null) {
        //this.searchedbookings = null;
        // console.log("heere" , this.choosengame.GameName)
        this.getBookingsbyname(this.choosengame.GameName,this.SiteID);
      }
      else if (this.choosendate.date != null) {
       // this.searchedbookings = null
        console.log("hello", this.choosendate.date)
        console.log("hello2", moment(this.choosendate.date).format('DD-MM-yyyy'))
        this.getBookingsbydate(moment(this.choosendate.date).format('DD-MM-yyyy'),this.SiteID)
      }

    }


  },
  created() {
    if (localStorage.length == 0) { this.$router.push({ name: 'login' }) }
    //  console.log(moment(this.today).format('DMMyyyy'));
    this.getSiteGames();
    this.getBookings();
    this.getSites();
    this.getPersons();
    this.getGames();
    this.getSchedules();
  }
}
</script>

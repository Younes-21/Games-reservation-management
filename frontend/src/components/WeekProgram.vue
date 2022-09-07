<template>

  <hr />
  <div class="wrap">
    <div class="left">
      <DayPilotNavigator id="nav" :config="navigatorConfig" />
    </div>
    <div class="content">
      <DayPilotCalendar id="dp" :config="config" ref="WeekProgram" />
    </div>
  </div>
</template>

<script>
import { DayPilot, DayPilotCalendar, DayPilotNavigator } from '@daypilot/daypilot-lite-vue'
import moment from 'moment'
export default {
  name: 'WeekProgram',
  data: function () {
    return {
      schedule: [],
      numberbookings: [],
      numberreservations: [],
      numberbooking: [],
      bookings: [],
      siite: localStorage.SiteIDAdmin,
      schedulebyid: [],
      sites: [],
      Now: moment().format(),
      PersonRole: localStorage.PersonRole,
      SiteID: localStorage.SiteID,
      PersonID: localStorage.PersonID,
      BookingDate: moment().format('MM-DD-YYYY'),
      schedules: [],
      events: [],
      navigatorConfig: {
        showMonths: 1,
        skipMonths: 1,
        selectMode: "Week",
        startDate: new Date(),
        selectionDay: new Date(),
      },
      config: {
        viewType: "Week",
        startDate: new Date(),
        durationBarVisible: true,
        timeRangeSelectedHandling: "Disabled",
        headerDateFormat: "dddd MMMM d, yyyy",
        eventMoveHandling: "Disabled",//optional
        eventResizeHandling: "Disabled",//optional
        eventEditHandling: "PostBack",

        onEventClick: async (args) => {
          if (args.e.data.start > this.Now) {
            var resp = await fetch('http://localhost:61917/api/booking/bookingexist/' + args.e.data.id + '/' + this.PersonID);
            this.numberreservations = await resp.json();
            console.log(this.numberreservations)
            console.log("right here", args.e)
            var res = await fetch('http://localhost:61917/api/booking/nbrbooking/' + args.e.data.id);
            this.numberbookings = await res.json();
            var re = await fetch('http://localhost:61917/api/schedule/' + args.e.data.id);
            this.schedule = await re.json();
            console.log("number reservation", this.numberreservations[0].Column1)
            console.log("SiteID", this.SiteID)
            console.log("siite", this.siite)
            console.log("numberbookings", this.numberbookings[0].Column1)
            console.log("availableseats", this.schedule)
            if (this.numberreservations[0].Column1 == 0 && this.SiteID == this.siite && this.numberbookings[0].Column1 < this.schedule[0].Availableseats) {
              const modal = await DayPilot.Modal.confirm("Confirmer vôtre reservation ", { top: 100, width: 400, theme: "modal_rounded" });
              console.log(modal.result);
              if (modal.canceled) {
                console.log("canceled")
              }
              else if (modal.result) {
                console.log("confirmed")
                var response = await fetch('http://localhost:61917/api/booking/insert/' + args.e.data.id + '/' + this.PersonID + '/' + this.BookingDate, {
                  method: 'post',
                  headers: {
                    'Content-Type': 'application/json'
                  },
                  body: JSON.stringify(this.schedule)
                });
                this.schedules.push(await response.json());
                window.location.href = '/mybookings'
              }
            }
            //alert("Vous ne pouvez pas effectuer cette réservation ")
            else {
              DayPilot.Modal.alert("Vous ne pouvez pas effectuer cette réservation", { top: 100, width: 400, theme: "modal_rounded" });
            }
          }
        },

      },
    }
  },
  props: {
  },
  components: {
    DayPilotCalendar,
    DayPilotNavigator
  },
  computed: {
    // DayPilot.Calendar object - https://api.daypilot.org/daypilot-calendar-class/
    calendar() {
      return this.$refs.WeekProgram.control;
    }
  },
  methods: {
    /* async getScheduleByid(id){
 var resp = await fetch('http://localhost:61917/api/schedule/'+id);
   this.schedulebyid = await resp.json();
 console.log("Voulu",this.schedulebyid[0].ScheduleID)
    },*/
    async loadEvents() {
      // placeholder for an HTTP call

      var response = await fetch('http://localhost:61917/api/schedule/schedulebysite/' + this.SiteID);
      this.schedules = await response.json();
      const events = []

      for (let i = 0; i < this.schedules.length; i++) {

        var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + this.schedules[i].ScheduleID);
        this.numberbooking = await resp.json();
        console.log(this.numberbooking)
        for (let j = 0; j < this.games.length; j++) {
          if (this.schedules[i].GameID == this.games[j].GameID) {

            this.GameName = this.games[j].GameName
            //console.log("here")
            //console.log(this.GameName)
          }
        }
        if (this.schedules[i].BeginDate >= this.Now) {
          events.push(
            {
              id: this.schedules[i].ScheduleID,
              start: this.schedules[i].BeginDate,
              end: this.schedules[i].EndDate,
              text: this.GameName + '\t' + this.numberbooking[0].Column1 + '/' + this.schedules[i].Availableseats,
              backColor: "#459EB0",
              borderColor: "#38761d",
              barColor: "rgb(13,85,107)", 

              areas: [
                { onClick: function (args) { const dp = args.control; dp.events.remove(args.source); }, width: 15, height: 15, top: 3, right: 3, cssClass: "event_action_delete" },
                { action: "ContextMenu", width: 15, height: 15, top: 3, right: 20, cssClass: "event_action_menu" }
              ]
            }
          )
          this.calendar.update({ events });
        }
        else {
          events.push(
            {
              id: this.schedules[i].ScheduleID,
              start: this.schedules[i].BeginDate,
              end: this.schedules[i].EndDate,
              text: this.GameName + '\t' + this.numberbooking[0].Column1 + '/' + this.schedules[i].Availableseats,

              backColor: "grey",
              barColor: "grey",
              resizeDisabled: true,
              moveDisabled: true,
              clickDisabled: true,
              deleteDisabled: true,
              areas: [
                { onClick: function (args) { const dp = args.control; dp.events.remove(args.source); }, width: 15, height: 15, top: 3, right: 3, cssClass: "event_action_delete" },
                { action: "ContextMenu", width: 15, height: 15, top: 3, right: 20, cssClass: "event_action_menu" }
              ]
            }
          )
          this.calendar.update({ events });
        }

      }

    },
    async getGames() {
      var response = await fetch('http://localhost:61917/api/game');
      this.games = await response.json();
    },

    async getSites() {
      var response = await fetch('http://localhost:61917/api/site');
      this.sites = await response.json();
    },
    async getBookings() {
      var response = await fetch('http://localhost:61917/api/booking');
      this.bookings = await response.json();
    },
  },
  mounted() {
    if (localStorage.length == 0) { this.$router.push({ name: 'login' }) }
    this.getGames();
    this.loadEvents();
    this.getSites();
    this.getBookings();
  }
}
</script>

<style>
.wrap {
  display: flex;
}

.left {
  margin-right: 10px;
}

.content {
  flex-grow: 1;
}


.calendar_default_event_inner {
  background: #2e78d6;
  color: white;
  border-radius: 5px;
  opacity: 0.9;
}

/*confirmation form css*/
.modal_rounded_background {
  background-color: #000;
  opacity: 0.75;
}

.modal_rounded_main {
  border-radius: 4px;
}

.modal_rounded_main,
.modal_rounded_main input,
.modal_rounded_main button {
  font-size: 14px;
}

.modal_rounded_inner {
  padding: 20px;
  background: #fff;
  color: #000;
  border-radius: 8px;
}

.modal_rounded_main input,
.modal_rounded_main button {
  padding: 5px;
  box-sizing: border-box;
  border-radius: 4px;
  border: 1px solid #ccc;
}

.modal_rounded_content {
  margin: 10px 0px;
}

.modal_rounded_input {
  margin: 20px 0px;
}

.modal_rounded_buttons {
  margin-top: 30px;
  margin-bottom: 10px;
}

.modal_rounded_main button {
  background-color: #2367bf;
  color: white;
  padding: 10px 0px;
  border: 0px;
  cursor: pointer;
  outline: none;
  width: 100px;
}

button.modal_rounded_cancel {
  margin-left: 10px;
  border: 1px solid #2367bf;
  background-color: #fff;
  color: #2367bf;
}
</style>

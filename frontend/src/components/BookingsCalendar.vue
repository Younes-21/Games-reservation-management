<template>
  <div class="wrap">
    <div class="left">
      <DayPilotNavigator id="nav" :config="navigatorConfig" />
    </div>
    <div class="content">
      <DayPilotCalendar id="dp" :config="config" ref="BookingsCalendar" />
    </div>
  </div>
</template>

<script>
import { DayPilot, DayPilotCalendar, DayPilotNavigator } from '@daypilot/daypilot-lite-vue'
import moment from 'moment';
export default {
  name: 'BookingsCalendar',
  data: function () {
    return {
      BeginDate: null,
      EndDate: null,
      GameName: null,
      PersonID: localStorage.PersonID,
      schedules: [],
      Now: moment().format(),
      bookings: [],
      games: [],
      events: [],
      navigatorConfig: {
        showMonths: 1,
        skipMonths: 1,
        selectMode: "Week",
        startDate: new Date(),
        selectionDay: new Date(),
      },
      config: {
        eventClickHandling: "Disabled",
        eventMoveHandling: "Disabled",
        eventResizeHandling: "Disabled",
        viewType: "Week",
        startDate: new Date(),
        durationBarVisible: true,
        timeRangeSelectedHandling: "Disabled",
        headerDateFormat: "dddd MMMM d, yyyy",

        eventDeleteHandling: "PostBack",
        onEventDelete: async (args) => {
          const modal = await DayPilot.Modal.confirm("Annuler la reservation ", { top: 100, width: 400, theme: "modal_rounded" });
          console.log(modal.result);
          if (modal.canceled) {
            console.log("canceled")
          }
          else if (modal.result) {
            console.log("confirmed")

            var response = await fetch("http://localhost:61917/api/booking/delete/" + args.e.data.id, {
              method: 'delete',
              headers: {
                'Content-Type': 'application/json'
              },
              body: JSON.stringify(this.schedule)
            });
            this.schedules.push(await response.json());
            window.location.reload(false);
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
      return this.$refs.BookingsCalendar.control;
    }
  },
  methods: {
    async loadEvents() {
      console.log("here")
      var response = await fetch('http://localhost:61917/api/booking/byperson/' + this.PersonID);
      this.bookings = await response.json();
      var res = await fetch('http://localhost:61917/api/game');
      this.games = await res.json();
      var resp = await fetch('http://localhost:61917/api/schedule');
      this.schedules = await resp.json();
      console.log(this.bookings)
      console.log(this.schedules)
      console.log(this.games)
      const events = []
      for (let i = 0; i < this.bookings.length; i++) {
        for (let j = 0; j < this.schedules.length; j++) {
          if ((this.bookings[i].ScheduleID == this.schedules[j].ScheduleID)) {
            //if(this.schedules[i].EndDate >= this.Now){
            this.BeginDate = this.schedules[j].BeginDate;
            this.EndDate = this.schedules[j].EndDate;
            for (let k = 0; k < this.games.length; k++) {
              // console.log(this.games[k].GameID)
              //console.log(this.schedules[j].GameID)

              if (this.games[k].GameID === this.schedules[j].GameID) {
                console.log(this.games[k].GameID)
                console.log(this.schedules[j].GameID)
                this.GameName = this.games[k].GameName;
              }
            }
            // }
          }
        }
        if (this.BeginDate >= this.Now) {
          events.push(
            {
              id: this.bookings[i].BookingID,
              start: this.BeginDate,
              end: this.EndDate,
              text: this.GameName,
              backColor: "#459EB0",
              barColor: "rgb(13,85,107)", 
              borderColor: "#38761d",
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
              id: this.bookings[i].BookingID,
              start: this.BeginDate,
              end: this.EndDate,
              text: this.GameName,
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
  },
  mounted() {
    if (localStorage.length == 0) { this.$router.push({ name: 'login' }) }
    this.loadEvents();
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
</style>

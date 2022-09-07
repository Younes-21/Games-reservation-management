<template>

  <hr />
  <div class="wrap">
    <div class="left">

      <DayPilotNavigator id="nav" :config="navigatorConfig" />

    </div>
    <div class="content">
      <DayPilotCalendar id="dp" :config="config" ref="CalendarVue" />
    </div>
  </div>
</template>

<script>
import { DayPilot, DayPilotCalendar, DayPilotNavigator } from '@daypilot/daypilot-lite-vue';
import moment from 'moment'
//AA_10122_CO
export default {
  name: 'CalendarVue',
  data: function () {
    return {
      numberbookings: [],
      numberbooking: [],
      found1: false,
      schedulebyid: [],
      found: false,
      bookings: [],
      schedulestofind: [],
      Now: moment().format(),
      PersonRole: localStorage.PersonRole,
      sites: {},
      SiteID: localStorage.SiteID,
      scheduletoedit: [],
      enter: true,
      data: {},
      PersonID: localStorage.PersonID,
      AddDate: moment().format('MM-DD-YYYY'),
      GameName: null,
      games: [],
      schedule: {},
      schedules: [],
      events: [],
      navigatorConfig: {
        showMonths: 1,
        skipMonths: 1,
        selectMode: "Week",
        onTimeRangeSelected: args => {
          this.config.startDate = args.day;
        },

      },
      config: {
        viewType: "Week",
        startDate: new Date(),
        durationBarVisible: true,
        timeRangeSelectedHandling: "Enabled",
        headerDateFormat: "dddd MMMM d, yyyy",
        onTimeRangeSelected: async (args) => {

          if (args.start >= this.Now) {
            var games = []
            for (let i = 0; i < this.games.length; i++) {
              if (this.games[i].Status == true && this.games[i].SiteID == this.SiteID) {
                games.push(
                  { name: this.games[i].GameName, id: this.games[i].GameID }
                )
              }
            }


            var form = [

              { name: "Places disponible", id: "Availableseats" /*, onValidate: validateTextRequired*/ },
              { name: "Jeu", id: "GameID", type: "select", options: games /*,  onValidate: validateTextRequired*/ },// onValidate: validateTextRequired

            ];
            this.data = {
              BeginDate: args.start,
              EndDate: args.end,
              Availableseats: null,
              GameID: null,
            };
            console.log(this.games)
            args = await DayPilot.Modal.form(form, this.data, { top: 100, width: 350, theme: "modal_rounded" });
            if (!args.canceled) {


              var response = await fetch("http://localhost:61917/api/schedule/insert/" + this.AddDate + "/" + moment(args.result.BeginDate).format('DDMMyyyyTHHmm')

                + "/" + moment(args.result.EndDate).format('DDMMyyyyTHHmm') + "/" + args.result.Availableseats + "/" + this.PersonID + "/" + args.result.GameID, {
                method: 'post',
                headers: {
                  'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.schedule)
              });
              this.schedules.push(await response.json());

              window.location.reload(false);
            }
            this.calendar.clearSelection();

          }
          this.calendar.clearSelection();
        },
        eventEditHandling: "PostBack",
        onEventClick: async (args) => {
   console.log("ici aaargs",args)
          var response = await fetch("http://localhost:61917/api/schedule/" + args.e.data.id);
          this.scheduletoedit = await response.json();
          var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + args.e.data.id);
          this.numberbookings = await resp.json();
          console.log(this.scheduletoedit)
          console.log("here", this.scheeduletoedit.ScheduleID)
          if (args.e.data.start >= this.Now) {
            console.log("dkhl")
            var games = []
            for (let i = 0; i < this.games.length; i++) {
              if (this.games[i].Status == true && this.games[i].SiteID == this.SiteID) {
                games.push(
                  { name: this.games[i].GameName, id: this.games[i].GameID }
                )
              }
            }


            var form = [

              { name: "Places disponible", id: "Availableseats" /*, onValidate: validateTextRequired*/ },
              { name: "Jeu", id: "GameID", type: "select", options: games, disabled: true/*, onValidate: validateTextRequired*/ },

            ];

            this.data = {
              BeginDate: this.scheeduletoedit.BeginDate,
              EndDate: this.scheeduletoedit.EndDate,
              Availableseats: this.scheeduletoedit.Availableseats,
              GameID: this.scheeduletoedit.GameID,
            };
            if (this.enter) {


              const arg = await DayPilot.Modal.form(form, this.data, { top: 100, width: 350, theme: "modal_rounded" });
              if (arg.result.Availableseats > this.numberbookings[0].Column1) {
                var res = await fetch('http://localhost:61917/api/schedule/update/' + args.e.data.id + "/" + moment(arg.result.BeginDate).format('DDMMyyyyTHHmm') + "/" + moment(arg.result.EndDate).format('DDMMyyyyTHHmm') + "/" + arg.result.Availableseats + "/" + arg.result.GameID, {
                  method: 'put',
                  headers: {
                    'Content-Type': 'application/json'
                  },
                  body: JSON.stringify(this.schedule)
                });
                this.schedules.push(await res.json());
                window.location.reload(false);
              }
              else {
                this.enter = false
                DayPilot.Modal.alert("Impossible de réaliser cette modification", { top: 100, width: 400, theme: "modal_rounded" }).then(function (args) {
                  if (args.result) {
                    console.log("You clicked OK.");
                    window.location.reload(false);
                  }
                  else {
                    console.log("You canceled the modal dialog.");
                    window.location.reload(false);
                  }
                });
              }
            }
          }
        },
        // EventClickHandling:"JavaScript",
        // EventClickJavaScript:"editEvent(e.id())",
        eventDeleteHandling: "PostBack",
        onEventDelete: async (args) => {
          for (let i = 0; i < this.bookings.length; i++) {
            if (this.bookings[i].ScheduleID == args.e.data.id) {
              this.found = true;
            }
          }
          if (this.found == false) {
            console.log(args.e.data.id)
            this.enter = false
            const modal = await DayPilot.Modal.confirm("Vous allez supprimer cet evenement", { top: 100, width: 400, theme: "modal_rounded" });
            console.log(modal.result);
            if (modal.canceled) {
              console.log("canceled")
              window.location.reload(false);
            }
            else if (modal.result) {
              console.log("confirmed")

              var response = await fetch("http://localhost:61917/api/schedule/delete/" + args.e.data.id, {
                method: 'delete',
                headers: {
                  'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.schedule)
              });
              this.schedules.push(await response.json());
              window.location.reload(false);
            }
          }
          else if (this.found == true) {
            this.enter = false
            DayPilot.Modal.alert("Suppression impossible  : évenement dèja réservé", { top: 100, width: 400, theme: "modal_rounded" }).then(function (args) {
              if (args.result) {
                console.log("You clicked OK.");
                window.location.reload(false);
              }
              else {
                console.log("You canceled the modal dialog.");
                window.location.reload(false);
              }
            });

          }

        },
        onEventMoved: async (args) => {
          console.log("aargs", args.e.data.start)
          console.log("now", this.Now)
          if (args.e.data.start > this.Now) {
            var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + args.e.data.id);
            this.numberbookings = await resp.json();
            if (this.numberbookings[0].Column1 == 0) {
              var response = await fetch("http://localhost:61917/api/schedule/" + args.e.data.id);
              //var response = await fetch("http://localhost:61917/api/schedule/bysite/"+this.SiteID);
              this.scheduletoedit = await response.json();
              var form = [
                { name: "Date début", id: "BeginDate", type: "date", dateFormat: "MMMM/d/yyyy hh:mm:ss", disabled: true },
                { name: "Date fin", id: "EndDate", type: "date", dateFormat: "MMMM/d/yyyy hh:mm:ss", disabled: true },
                //{name: "Color", id: "barColor", type: "select", options: colors},
              ];
              this.data = {
                BeginDate: args.newStart,
                EndDate: args.newEnd,
              };

              if (this.enter) {
                const arg = await DayPilot.Modal.form(form, this.data, { top: 100, width: 400, theme: "modal_rounded" });
                if (arg.result) {
                  var res = await fetch('http://localhost:61917/api/schedule/updatetime/' + args.e.data.id + '/' +
                    moment(arg.result.BeginDate).format('DDMMyyyyTHHmm')
                    + "/" + moment(arg.result.EndDate).format('DDMMyyyyTHHmm'), {
                    method: 'put',
                    headers: {
                      'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(this.schedule)
                  });
                  this.schedules.push(await res.json());
                  window.location.reload(false);
                }
                else {
                  window.location.reload(false);
                }
              }
            }
            else {
              DayPilot.Modal.alert("Modification impossible  : évenement dèja réservé", { top: 100, width: 400, theme: "modal_rounded" }).then(function (args) {
                if (args.result) {
                  console.log("You clicked OK.");
                  window.location.reload(false);
                }
                else {
                  console.log("You canceled the modal dialog.");
                  window.location.reload(false);
                }
              });
            }
          }
          else {
            DayPilot.Modal.alert("Cette date est dèja passée : impossible de mettre cet évenement ici", { top: 100, width: 500, theme: "modal_rounded" }).then(function (args) {
              if (args.result) {
                console.log("You clicked OK.");
                window.location.reload(false);
              }
              else {
                console.log("You canceled the modal dialog.");
                window.location.reload(false);
              }
            });
          }
        }, onEventResized: async (args) => {
          console.log("args", args)
          var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + args.e.data.id);
          this.numberbookings = await resp.json();
          if (this.numberbookings[0].Column1 == 0 && (args.e.data.start > this.Now)) {

            var response = await fetch("http://localhost:61917/api/schedule/" + args.e.data.id);
            this.scheduletoedit = await response.json();
            var form = [
              { name: "Date début", id: "BeginDate", type: "date", dateFormat: "MMMM/d/yyyy hh:mm:ss", disabled: true },
              { name: "Date fin", id: "EndDate", type: "date", dateFormat: "MMMM/d/yyyy hh:mm:ss", disabled: true },
              //{name: "Color", id: "barColor", type: "select", options: colors},
            ];
            this.data = {
              BeginDate: args.newStart,
              EndDate: args.newEnd,
            };

            if (this.enter) {
              const arg = await DayPilot.Modal.form(form, this.data, { top: 100, width: 400, theme: "modal_rounded" });
              if (arg.result) {
                var res = await fetch('http://localhost:61917/api/schedule/updatetime/' + args.e.data.id + '/' +
                  moment(arg.result.BeginDate).format('DDMMyyyyTHHmm')
                  + "/" + moment(arg.result.EndDate).format('DDMMyyyyTHHmm'), {
                  method: 'put',
                  headers: {
                    'Content-Type': 'application/json'
                  },
                  body: JSON.stringify(this.schedule)
                });
                this.schedules.push(await res.json());
                window.location.reload(false);
              }
              else {
                window.location.reload(false);
              }
            }
          }
          else if (this.numberbookings[0].Column1 != 0) {
            DayPilot.Modal.alert("Modification impossible  : évenement dèja réservé", { top: 100, width: 400, theme: "modal_rounded" }).then(function (args) {
              if (args.result) {
                console.log("You clicked OK.");
                window.location.reload(false);
              }
              else {
                console.log("You canceled the modal dialog.");
                window.location.reload(false);
              }
            });
          }
          else if (args.e.data.start < this.Now) {
            DayPilot.Modal.alert("Modification impossible  : Date dèja passée", { top: 100, width: 400, theme: "modal_rounded" }).then(function (args) {
              if (args.result) {
                console.log("You clicked OK.");
                window.location.reload(false);
              }
              else {
                console.log("You canceled the modal dialog.");
                window.location.reload(false);
              }
            });
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
    scheeduletoedit() {
      return this.scheduletoedit[0]
    },
    calendar() {
      return this.$refs.CalendarVue.control;
    }
  },
  methods: {
    async getScheduleByid(id) {
      var resp = await fetch('http://localhost:61917/api/schedule/' + id);
      this.schedulebyid = await resp.json();
      //console.log("Voulu",this.schedulebyid[0].ScheduleID)
    },
    async getBookings() {
      var response = await fetch('http://localhost:61917/api/booking');
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

    async loadEvents() {


      // console.log("here")
      var response = await fetch('http://localhost:61917/api/schedule/schedulebysite/' + this.SiteID);
      this.schedules = await response.json();
      console.log(this.schedules)
      const events = []
      //console.log("daypilotdate",this.Now)
      //console.log("Schedule",this.schedules[0].BeginDate)
      for (let i = 0; i < this.schedules.length; i++) {

        var resp = await fetch('http://localhost:61917/api/booking/nbrbooking/' + this.schedules[i].ScheduleID);
        this.numberbooking = await resp.json();
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
              barColor: "rgb(13,85,107)",
              backColor: "#459EB0",
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

  },
  mounted() {
    if (localStorage.length == 0) { this.$router.push({ name: 'login' }) }
    console.log("personrole", this.PersonRole)
    this.getGames();
    this.loadEvents();
    this.getSites();

    this.getBookings()
  },


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

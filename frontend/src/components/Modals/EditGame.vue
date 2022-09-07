<template>
  <div class="bloc-modale">
    <div class="overlay" v-on:click="toggleModalUpdate"></div>
    <div class="modale card">

      <div class="container">
        <center>
          <h2 style="color:#1D4851;"><b>Modifier le jeu</b></h2>
        </center>
        <form @submit.prevent="editGame">

          <div class="form-row" style="margin-left: 7rem;">
            <div class="input-data">
              <input type="text" style="width: 155px;" v-model="game.GameName" required>
              <div class="underline"></div>
              <label style="left:0px; color:black;">Nom du jeu</label>
            </div>
          </div>


          <div class="form-row" style="margin-left:8rem;">
            <label style="left:0px; color:black;">&nbsp;&nbsp;Disponibilité &nbsp;&nbsp;</label>
            <input class="form-check-input" style="margin-top: 4px;" type="checkbox" value="" id="flexCheckDefault"
              v-model="game.Status">
          </div>
          <div class="form-row submit-btn">
            <div class="input-data">
              <div class="inner"></div>
              <input type="submit" style="margin: 0;width: 120px; position: absolute;top: 50%; left: 110%;
                       right:99%; -ms-transform: translate(-50%, -50%);transform: translate(-50%, -50%);
                        background-color:#1D4851;color: white;border-radius: 8px;" value="Valider">
            </div>
            <div class="input-data">
              <div class="inner"></div>
              <button type="button" class="btn btn-danger submit-btn"
                style="border-radius: 8px;margin-left:30%; height: 40px; width: 120px;"
                @click="toggleModalUpdate">Annuler</button>
            </div>
          </div>
        </form>

      </div>

    </div>
  </div>
</template>
<script>
export default {
  data() {
    return {
      //game : this.gametoedit,
      games: []
    };
  },
  computed: {
    game() {
      return this.gametoedit[0]
    }
  },
  methods: {
    async editGame() {

      var response = await fetch("http://localhost:61917/api/game/update/" + this.game.GameID + "/" + this.game.GameName + "/" + this.game.SiteID + "/" + this.game.Status, {
        method: 'put',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(this.game)
      });
      this.games.push(await response.json());
      window.location.reload(false);
    },
  },
  props: ['showupdategame', 'toggleModalUpdate', 'gametoedit'],
  created() {
    console.log('teest', this.game.GameName)
  }
}
</script>
<style scoped>
/* Modal css*/
.bloc-modale {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  display: flex;
  justify-content: center;
  align-items: center;
}

.overlay {
  background: rgba(0, 0, 0, 0.5);
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
}

.modale {
  background: #f1f1f1;
  color: #333;
  padding: 15px;
  position: fixed;
  top: 10%;
  left: 30%;


}

.btn-modale {
  position: absolute;
  top: 10px;
  right: 10px;
}

/*Form css*/
* {
  margin: 0;
  padding: 0;
  outline: none;
  box-sizing: border-box;
  font-family: 'Poppins', sans-serif;
}

body {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 40px;

}

.container {
  max-width: 800px;
  background: #fff;
  width: 500px;
  padding: 25px 40px 10px 40px;
  box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
}

.container form {
  padding: 15px 0 0 0;
}

.container form .form-row {
  display: flex;
  margin: 32px 0;
}

form .form-row .input-data {
  width: 30%;
  height: 40px;
  margin: 0 20px;
  position: relative;
}

form .form-row .textarea {
  height: 70px;
}

.input-data input,
.textarea textarea {
  display: block;
  width: 70%;
  height: 100%;
  border: none;
  font-size: 17px;
  border-bottom: 2px solid rgba(0, 0, 0, 0.12);
}

.input-data input:focus~label,
.textarea textarea:focus~label,
.input-data input:valid~label,
.textarea textarea:valid~label {
  transform: translateY(-20px);
  font-size: 14px;

}

.textarea textarea {
  resize: none;
  padding-top: 10px;
}

.input-data label {
  position: absolute;
  pointer-events: none;
  bottom: 10px;
  font-size: 16px;
  transition: all 0.3s ease;
}

.textarea label {
  width: 100%;
  bottom: 40px;
  background: #fff;
}

.input-data .underline {
  position: fixed;
  bottom: 0;
  height: 2px;
  width: 100%;
}

.input-data input:focus~.underline:before,
.input-data input:valid~.underline:before,
.textarea textarea:focus~.underline:before,
.textarea textarea:valid~.underline:before {
  transform: scale(1);
}
</style>
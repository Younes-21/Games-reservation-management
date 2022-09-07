<template >

  <head>
    <!-- Design by foolishdeveloper.com -->
    <title>Login</title>

    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;500;600&display=swap" rel="stylesheet">
    <!--Stylesheet-->

  </head>

  <body>
    <div class="background">
      <div class="shape"></div>
      <div class="shape"></div>
    </div>
    <form @submit.prevent="Login">
      <h3>
        <center style="color: rgb(5, 45, 66);">Authentification</center>
      </h3>
      <label for="username">Email</label>
      <input type="text" placeholder="Email" id="email" v-model="person.email" required>

      <label for="password">Mot de passe</label>
      <input type="password" placeholder="Mot de passe" id="password" v-model="person.password" required>
      <div v-if="!connect" class="alert alert-danger text-center" role="alert" style="height:60px; margin-top: 15px;">
        <span style=" font-size: 15px;">
          Email ou mot de passe incorrecte!!
        </span>
      </div>
      <button class="btn btn-light" style="color: rgb(5, 45, 66);">Se connecter</button>
      &nbsp;
      &nbsp;
      &nbsp;
      <center>© 2022 WebHelp</center>

    </form>

  </body>
</template>

<script >
export default {
  data() {
    return {
      role: null,
      personjobs: [],
      connect: true,
      person: {},
      persons: [],
      password: "123",
    }
  },
  methods: {
    async GetPersons() {
      var response = await fetch('http://localhost:61917/api/person');
      this.persons = await response.json();
    },
    async GetPersonJobs() {
      var response = await fetch('http://localhost:61917/api/personjob');
      this.personjobs = await response.json();
    },
    async Login() {

      var found = false;
      for (let i = 0; i < this.persons.length; i++) {
        if (this.persons[i].Email == this.person.email && this.person.password == this.password) {
          localStorage.setItem('PersonID', this.persons[i].PersonID)
          localStorage.setItem('PersonRole', this.persons[i].PersonRole)
          this.role = this.persons[i].PersonRole;
          for (let j = 0; j < this.personjobs.length; j++) {
            if (this.personjobs[j].PersonID == this.persons[i].PersonID) {
              localStorage.setItem('SiteID', this.personjobs[j].JobSiteID)
              localStorage.setItem('SiteIDAdmin', this.personjobs[j].JobSiteID)
            }
          }
          found = true;
          break;
        }
      }
      if (found == true) {
        this.connect = found;
        console.log("connected");
        window.location.href = '/gamesview'
      }
      else {
        this.connect = false;
        console.log("refused")
      }
      if (this.role == 1 || this.role == 2) {
        window.location.href = '/gamesview'
      }
      else if (this.role == 0) {
        window.location.href = '/schedule'
      }

    }
  },
  created() {
    this.GetPersons();
    this.GetPersonJobs();
  }
}

</script>
<style scoped>
body {
  background-color: #f5ebeb;
  background-image: url("../assets/webhelp.jpg");
  background-size: 172px;
  background: "../assets/webhelp.jpg";
}
</style>
<style scoped>
*,
*:before,
*:after {
  padding: 0;
  margin: 0;
  box-sizing: border-box;
}



.background {
  width: 100%;
  height: 100%;
  position: absolute;
  background-repeat: repeat;
  background-size: 153px 103px;

  right: 0%;
  top: 0%;
  background-image: url("../assets/webhelp.jpg");
}

form {
  height: 520px;
  width: 400px;
  background-color: white;
  position: absolute;
  transform: translate(-50%, -50%);
  top: 50%;
  left: 50%;
  border-radius: 10px;
  backdrop-filter: blur(10px);
  border: 2px solid rgba(7, 48, 70, 0.066);
  box-shadow: 0 0 40px rgba(8, 7, 16, 0.6);
  padding: 50px 35px;
}

form * {
  font-family: 'Poppins', sans-serif;
  color: #000000ed;
  letter-spacing: 0.5px;
  outline: none;
  border: none;
}

form h3 {
  font-size: 32px;
  font-weight: 500;
  line-height: 42px;
  text-align: center;
}

label {
  display: block;
  margin-top: 30px;
  font-size: 16px;
  font-weight: 500;
}

input {
  display: block;
  height: 50px;
  width: 100%;
  background-color: rgba(255, 255, 255, 0.07);
  border-radius: 3px;
  border-style: solid;
  border-width: 1px;
  padding: 0 10px;
  margin-top: 8px;
  font-size: 14px;
  font-weight: 300;
}

::placeholder {
  color: #1d1c1c;
}

button {
  margin-top: 10px;
  width: 100%;
  background-color: #ffffff7a;
  color: #080710;
  padding: 15px 0;
  font-size: 18px;
  font-weight: 600;
  border-radius: 5px;
  cursor: pointer;
}

.social {
  margin-top: 30px;
  display: flex;
}

.social div {
  background: red;
  width: 150px;
  border-radius: 3px;
  padding: 5px 10px 10px 5px;
  background-color: rgba(255, 255, 255, 0.27);
  color: #000000;
  text-align: center;
}

.social div:hover {
  background-color: rgba(255, 255, 255, 0.47);
}

.social .fb {
  margin-left: 25px;
}

.social i {
  margin-right: 4px;
}
</style>
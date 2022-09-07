<template>

  <head>
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Bootstrap 5 404 Error Page</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
  </head>


  <body>
    <div class="d-flex align-items-center justify-content-center vh-100">
      <div class="text-center">
        <h1 class="display-1 fw-bold">404</h1>
        <p class="fs-3"> <span class="text-danger">Opps!</span> Page not found.</p>
        <p class="lead">
          The <b>{{  this.therror }}</b> page you’re looking for doesn’t exist .
        </p>
      </div>
    </div>
  </body>
</template>
    
    <script>
export default {
  data() {
    return {
      therror: null
    }

  },
  created() {
    this.therror = this.$route.params.therror;
  },
}
</script>
    <style  scoped>
    body {
      background-color: #95c2deb0;
    }
    
    .mainbox {
      background-color: #95c2de;
      margin: auto;
      height: 600px;
      width: 600px;
      position: relative;
    }
    
    .err {
      color: #ffffff;
      font-family: 'Nunito Sans', sans-serif;
      font-size: 11rem;
      position: absolute;
      left: 20%;
      top: 8%;
    }
    
    .far {
      position: absolute;
      font-size: 8.5rem;
      left: 42%;
      top: 15%;
      color: #ffffff;
    }
    
    .err2 {
      color: #ffffff;
      font-family: 'Nunito Sans', sans-serif;
      font-size: 11rem;
      position: absolute;
      left: 68%;
      top: 8%;
    }
    
    .msg {
      text-align: center;
      font-family: 'Nunito Sans', sans-serif;
      font-size: 1.6rem;
      position: absolute;
      left: 16%;
      top: 45%;
      width: 75%;
    }
    
    a {
      text-decoration: none;
      color: white;
    }
    
    a:hover {
      text-decoration: underline;
    }
    </style>
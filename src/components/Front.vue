<template>
  <div class="mt-3">
    <div class="banner">
      <div class="container h-100">
      <div class="front-box my-auto">
        <h1 class="fw-bold display-5">
          YOUR OPPORTUNITY FOR<br />
          <span class="fw-bold display-5 text-success">FINANCIAL INDEPENDENCE</span>
        </h1>
        <div class="d-flex w-50">
          <router-link class="btn btn-dark" to="/login">LOGIN</router-link>
          <router-link class="btn btn-dark text-nowrap" to="/about"
            >ABOUT US</router-link
          >
        </div>
      </div>
      </div>
    </div>
    <div class="container mt-5">
      <div class="row d-flex flex-row w-100">
        <div class="box-img col-4">
          <h4 class="fw-bold">HOUSE</h4>
          <img src="../assets/bolig.jpg" class="img-fluid" alt="Responsive image" />
          <h6>READ MORE</h6>
        </div>
        <div class="box-img col-4">
          <h4 class="fw-bold">INVESTMENT</h4>
          <img src="../assets/investering.jpg" class="img-fluid" alt="Responsive image" />
          <h6>READ MORE</h6>
        </div>
        <div class="box-img col-4">
          <h4 class="fw-bold">YOUNG</h4>
          <img src="../assets/ung.jpg" class="img-fluid" alt="Responsive image" />
          <h6>READ MORE</h6>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "FrontPage",
};
</script>

<style scoped>
.box-img img {
  border-bottom: 3px solid #343a40;
}
.box-img h6 {
  position: absolute;
  bottom: 10%;
  left: 50%;
  transform: translate(-50%, 0);
  color: white;
  text-shadow: #343a40 0px 0px 10px;
  border-bottom: 3px solid #343a40;
}
.box-img h4 {
  z-index: 1;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: white;
  text-shadow: #343a40 0px 0px 10px;
}
.box-img{
  position: relative;
  text-align: center;
  transition: 2s ease;
}

.box-img:hover {
  box-shadow: inset 0 0 0 1000px rgba(0, 0, 0, 0.5);
  
}
.banner {
  display: flex;
  justify-content: end;
  background-image: url("../assets/Banner.jpg");
  background-size: cover;
  height: 45vh;
  max-height: 335px;
}
.front-box {
  background-color: #f5f5f5;
  padding: 2rem;
  margin: 100px 20px;
  margin-top: 50px !important;
  border: 1px solid #e3e3e3;
  max-width: 60%;
}
.btn-dark {
  background-color: #343a40;
  border: 1px solid #343a40;
  margin-right: 10px;
  margin-top: 20px;
  width: 200px;
  height: 50px;
  border-radius: 0;
  font-size: 1.2rem;
  font-weight: 600;
  line-height: 36px;
}
</style>
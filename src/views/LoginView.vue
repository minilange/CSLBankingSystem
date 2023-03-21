<template>
  <div class="container">
    <div class="action-prompt">
      <h4 class="text-center fw-bold my-4">LOGIN</h4>
      <form>
        <div class="form mb-3">
          <label class="fw-bold" for="Email">EMAIL ADDRESS</label>
          <input v-model="form.email" type="email" class="form-control" id="Email"
          placeholder="Name@gmail.com" />
        </div>
        <div class="form mb-3">
          <label class="fw-bold" for="Password">PASSWORD</label>
          <input v-model="form.password" type="password" class="form-control" id="Password"
            placeholder="Password" />
        </div>
      </form>
      <button v-on:click="loginUser()" class="btn-transparent btn btn-dark" :disabled="$store.state.searching == true || form.password.length == 0 || form.email.length == 0" >LOGIN <span class="float-end"><font-awesome-icon icon="right-to-bracket" /></span></button>
      
      <div v-if="$store.state.searching == true" id="searchStatus" class="mt-4 d-flex">
        <span>Loggin in...</span>
        <div class="loader"></div>
      </div>

      <router-link class="text-center nav-link text-white" to="/register"><span>Need an account?</span></router-link>
    </div>
  </div>
</template>

<style scoped>
  .container{
    display: flex;
    align-items: center;
    justify-content: center;
    height: 90vh;
  }
.action-prompt{
  height: fit-content;
  width: 35%;
}

span {
  color: black;
}

@media (max-width: 991px) {
  .action-prompt {
    width: 80%;
  }
}
</style>

<script>
import { SHA256 } from "crypto-js";

export default {
  data() {
    return {
      form: {
        email: "",
        password: "",
      },
    };
  },
  mounted() {
    // Check if user is already logged in local storage
    if (localStorage.getItem("user")) {
      this.$store.commit("setUser", JSON.parse(localStorage.getItem("user")));
      alert("You are already logged in!")
      this.$router.push("/");
    }
  },
  methods: {
    loginUser: function () {
      console.log('loginUser')
      this.$store.dispatch("loginUser", {
        params: {
          email: this.form.email,
          password: SHA256(this.form.password, this.$store.state.secret).toString(),
          userId: this.$store.state.user.userId,
          token: this.$store.state.user.token,
        },
      })
    },
  },
};
</script>

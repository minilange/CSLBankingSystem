<script setup>
import Header from "./components/Header.vue";
import Footer from "./components/Footer.vue";
</script>

<template>
  <!-- <div class="container"> -->
    <Header />
    <RouterView />
    <Footer/>
  <!-- </div> -->
</template>

<style scoped>
Footer {
  position: fixed;
  bottom: 0;
  width: 100%;
}
</style>


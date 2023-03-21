<template>
  <div class="container mt-3">
    <div class="body">
      <div class="row">
        <div class="col-6">
          <h1>ACCOUNT OVERVIEW</h1>
          <hr />
        </div>
        <div class="col-6">
          <div class="account-list">
            <div class="account">
              <h4>ACCOUNT 1</h4>
              <p>ID: 101238</p>
            </div>
            <div class="account">
              <h4>ACCOUNT 2</h4>
              <p>ID: 101238</p>
            </div>
            <div class="account">
              <h4>ACCOUNT 3</h4>
              <p>ID: 101238</p>
            </div>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="col-6">
          <h3>TRANSACTIONS</h3>
          <table class="table table-striped table-hover">
            <caption>
              List of transactions
            </caption>
            <thead class="table-dark">
              <tr>
                <th class="fw-bold" scope="col">FromID</th>
                <th class="fw-bold" scope="col">ToID</th>
                <th class="fw-bold" scope="col">Amount</th>
                <th class="fw-bold" scope="col">Date</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="index in 10" :key="index">
                <td>123123</td>
                <td>461234</td>
                <td>75</td>
                <td>21/03/2023</td>
              </tr>
            </tbody>
          </table>
        </div>
        <div class="col-6">
          <div class="ms-4">
            <h3 class="mb-0 fw-bold">BALANCE</h3>
            <p class="id text-muted">ID: 101238</p>
            <p class="mb-0 total-balance">TOTAL BALANCE</p>
            <h4>$46000,00</h4>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.total-balance .id {
  font-size: 12px;
  color: #6c757d;
}
.body {
  background-color: white;
  padding: 20px;
  height: 90.8vh;
}

hr {
  height: 4px;
  background-color: black !important;
  opacity: 1;
}

.account-list {
  display: flex;
  flex-direction: row;
}
.account {
  display: flex;
  flex-direction: column;
  margin: 0px 20px 0px 20px;
}

.account > p {
  font-size: 12px;
  margin-bottom: 0px;
}
.account > h4 {
  margin-bottom: 0px;
}
</style>
  
<script>
export default {
  data() {
    return {};
  },
  computed: {},
  mounted() {
    this.getAccountData();
  },
  methods: {
    showAccountData(id) {
      console.log("show account data", id);
    },
    getAccountData() {
      console.log("get account data");
    },
  },
};
</script>
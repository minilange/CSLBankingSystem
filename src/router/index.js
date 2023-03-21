import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import AccountView from '../views/AccountView.vue'
// import store from '../store/index.js'

// function isLoggedIn() {
//   if(!Object.keys(store.state.user).length == 0){
//     return true
//   }else{
//     return '/'
//   }
// }
// function isAdmin() {
//   if(!Object.keys(store.state.user).length == 0){
//     try {
//       if(store.state.user.admin == true){
//         return true
//       }
//     } catch(e) {
//       console.log('isAdmin ', e)
//     }
//   }


//   return '/' // Default
// }

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/register',
    name: 'registers',
    component: RegisterView
  },
  {
    path: '/account',
    name: 'account',
    component: AccountView,
    // beforeEnter: [isLoggedIn]
  },
  {
    path: "/:catchAll(.*)",
    component: HomeView,
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router

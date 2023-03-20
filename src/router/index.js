import { createRouter, createWebHashHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import AboutView from '../views/AboutView.vue'
import LoginView from '../views/LoginView.vue'
import OldParkingsView from '../views/OldParkingsView.vue'
import NewParkingView from '../views/NewParkingView.vue'
import AreaView from '../views/AreaView.vue'
import RegisterView from '../views/RegisterView.vue'
import CarRegisterView from '../views/CarRegisterView.vue'
import ProfileView from '../views/ProfileView.vue'
import AdminView from '../views/AdminView.vue'
import store from '../store/index.js'

function isLoggedIn() {
  if(!Object.keys(store.state.user).length == 0){
    return true
  }else{
    return '/'
  }
}
function isAdmin() {
  if(!Object.keys(store.state.user).length == 0){
    try {
      if(store.state.user.admin == true){
        return true
      }
    } catch(e) {
      console.log('isAdmin ', e)
    }
  }


  return '/' // Default
}

const routes = [
  {
    path: '/',
    name: 'home',
    component: HomeView,
  },
  {
    path: '/about',
    name: 'about',
    component: AboutView
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/old-parkings',
    name: 'oldparkings',
    component: OldParkingsView,
    beforeEnter: [isLoggedIn]
  },
  {
    path: '/new-parking',
    name: 'newparking',
    component: NewParkingView,
    beforeEnter: [isLoggedIn]
  },
  {
    path: '/areas',
    name: 'areas',
    component: AreaView,
    beforeEnter: [isLoggedIn]
  },
  {
    path: '/register',
    name: 'registers',
    component: RegisterView
  },
  {
    path: '/register-car',
    name: 'carregister',
    component: CarRegisterView,
    beforeEnter: [isLoggedIn]
  },
  {
    path: '/profile',
    name: 'profile',
    component: ProfileView,
    beforeEnter: [isLoggedIn]
  },
  {
    path: '/admin',
    name: 'admin',
    component: AdminView,
    beforeEnter: [isAdmin] 
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

import { createApp } from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import { loadFonts } from './plugins/webfontloader'
import router from './router'
import axios from 'axios'


loadFonts()

axios.defaults.baseURL='http://localhost:63036/'

createApp(App)
  .use(router)
  .use(vuetify)
  .mount('#app')




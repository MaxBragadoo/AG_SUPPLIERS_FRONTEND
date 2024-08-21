// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'
import { VDateInput } from 'vuetify/labs/VDateInput'  //Esta exportacion se tuvo que hacer para poder ocupar los vDateImput
import { VNumberInput } from 'vuetify/lib/labs/components.mjs'

// Vuetify
import { createVuetify } from 'vuetify'

export default createVuetify({
  // https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides

  components: {  //Se tuvo que crear esto para poder usar los v-dateInput y los 
    VDateInput,
    VNumberInput,
  },

})

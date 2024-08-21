import { createRouter, createWebHashHistory } from "vue-router"
import CategoriasComponent      from "@/components/Almacen/CategoriasComponent.vue"
import ArticulosComponent       from "@/components/Almacen/ArticulosComponet.vue"
import HelloWorld               from "@/components/HelloWorld.vue"
import UnidadMedidasComponent   from "@/components/Almacen/UnidadMedidasComponent.vue"
import AlmacenesComponent       from "@/components/Almacen/AlmacenesComponent.vue"
import ProveedoresComponent     from "@/components/Almacen/ProveedoresComponent.vue"
import UbicacionesComponent     from "@/components/Almacen/UbicacionesComponent.vue"
import ComprasComponent         from "@/components/Compras/ComprasComponent.vue"
import VentasComponent          from "@/components/Salidas/VentasComponent.vue"

import Ot_modelo                from '../components/OrdenTrabajo/Ot_modelo.vue'
import Ot_aeronave              from '../components/OrdenTrabajo/Ot_aeronave.vue'
import Ot_programada            from '../components/OrdenTrabajo/Ot_programada.vue'
import Ot_encabezado_ot         from '../components/OrdenTrabajo/Ot_encabezado_ot.vue'
import Ot_cliente               from '../components/OrdenTrabajo/Ot_cliente.vue'

const routes = [
  {
    path: '/',
    name: 'home',
    component: HelloWorld,
      },

  {
    path: '/almacenes',
    name: 'almacenes',
    component: AlmacenesComponent
  },
  {
    path: '/ubicaciones',
    name: 'ubicaciones',
    component: UbicacionesComponent
  },

  {
    path: '/categorias',
    name: 'categorias',
    component: CategoriasComponent
  },
  {
    path: '/unidadMedidas',
    name: 'unidadMedidas',
    component: UnidadMedidasComponent
  },
  {
    path: '/articulos',
    name: 'articulos',
    component: ArticulosComponent
  },
  {
    path: '/proveedores',
    name: 'proveedores',
    component: ProveedoresComponent

  },
  {
    path: '/compras',
    name: 'compras',
    component: ComprasComponent

  },
  {
    path: '/ventas',
    name: 'ventas',
    component: VentasComponent
  },
  {
    path: '/Ot_modelo',
    name: 'Ot_modelo',   
    component: Ot_modelo
  },
  {
    path: '/Ot_aeronave',
    name: 'Ot_aeronave',   
    component: Ot_aeronave
  },
  {
    path: '/Ot_programada',
    name: 'Ot_programada',   
    component: Ot_programada
  },
  {
    path: '/Ot_cliente',
    name: 'Ot_cliente',   
    component: Ot_cliente
  },
  {
    path: '/Ot_encabezado_ot',
    name: 'Ot_encabezado_ot',   
    component: Ot_encabezado_ot
  },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes
})

export default router

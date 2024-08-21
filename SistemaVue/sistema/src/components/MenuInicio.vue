<template>
  <v-card>
    <v-layout>
      <v-app-bar color="#045c84" prominent>
        <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
        <v-toolbar-title>Sistema AG Aviation Suppliers</v-toolbar-title>
        <v-spacer></v-spacer>
        <template v-if="$vuetify.display.mdAndUp">
          <v-btn icon="mdi-magnify" variant="text"></v-btn>
          <v-btn icon="mdi-filter" variant="text"></v-btn>
        </template>
        <v-btn icon="mdi-dots-vertical" variant="text"></v-btn>
      </v-app-bar>

      <v-navigation-drawer 
        :width="350" 
        v-model="drawer" 
        :location="$vuetify.display.mobile ? 'bottom' : undefined" temporary
        >
        <v-list v-model:opened="open">
          <v-list-item prepend-icon="mdi-home" title="Menú de Opciones"></v-list-item>

          <v-list-group value="Almacen">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-warehouse" title="Almacén"></v-list-item>
            </template>
            <v-list-group value="Catalogos">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-folder-text-outline" title="Catalogos"></v-list-item>
            </template>              
              <v-list-item v-for="([title, icon, page], i) in almacenes" :key="i" :prepend-icon="icon" :title="title"
              :value="title" route :to="page"></v-list-item>            
          </v-list-group>
          <v-list-group value="Adquisiciones">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-dolly" title="Adquisiciones"></v-list-item>
            </template>              
              <v-list-item v-for="([title, icon, page], i) in adquisiciones" :key="i" :prepend-icon="icon" :title="title"
              :value="title" route :to="page"></v-list-item>            
          </v-list-group>
          <v-list-group value="Salidas">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-bank-transfer-out" title="Salidas"></v-list-item>
            </template>              
              <v-list-item v-for="([title, icon, page], i) in salidas" :key="i" :prepend-icon="icon" :title="title"
              :value="title" route :to="page"></v-list-item>            
          </v-list-group>

          </v-list-group>
          
          <v-list-group value="Orden Trabajo">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-warehouse" title="Orden Trabajo"></v-list-item>
            </template>

            <v-list-group value="Catalogos">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-folder-text-outline" title="Catalogos"></v-list-item>
            </template>              
            
            <v-list-item v-for="([title, icon, page], i) in catalogos_ot" :key="i" :prepend-icon="icon" :title="title"
              :value="title" route :to="page"></v-list-item>            
             </v-list-group>

          <v-list-group value="Gestion OT">
            <template v-slot:activator="{ props }">
              <v-list-item v-bind="props" prepend-icon="mdi-dolly" title="Gestion"></v-list-item>
            </template>              
              <v-list-item v-for="([title, icon, page], i) in gestion_ot" :key="i" :prepend-icon="icon" :title="title"
              :value="title" route :to="page"></v-list-item>            
          </v-list-group>

          </v-list-group>

        </v-list>
      </v-navigation-drawer>
      
      <v-main>
        <v-card-text >
          <!-- AQUI MANDAMOS LLAMAR A LAS RUTAS DE CADA COMPONENT -->
          <router-view></router-view>
          <!-- FIN DEL LLAMADO DE LAS RUTAS -->
        </v-card-text>
      </v-main>
    </v-layout>
  </v-card>
</template>

<script>
export default {
  name: 'App',
  data: () => ({
    drawer: false,
    group: null,
    open: ['Almacen'],
    almacenes: [
        ['Almacenes', 'mdi-warehouse','almacenes'],
        ['Ubicaciones', 'mdi-warehouse', 'ubicaciones'],
        ['Unidad de Medidas', 'mdi-unity','unidadMedidas'],
        ['Categorías','mdi-group','categorias'],
        ['Artículos', 'mdi-cart','articulos'],
        ['Proveedores', 'mdi-truck-delivery','proveedores']
    ], 
    adquisiciones: [
        ['Entradas', 'mdi-import','compras'],
    ],
    salidas: [
        ['Salidas OT', 'mdi-export','almacenes'],
    ],
    admins: [
        ['Categorias', 'mdi-account-multiple-outline'],
        ['Settings', 'mdi-cog-outline'],
      ],

    catalogos_ot: [
        ['Modelo', 'mdi-warehouse','modelos'],
        ['Aeronave', 'mdi-warehouse', 'aeronave'],
        ['Programada', 'mdi-unity','programada'],
        ['Cliente','mdi-group','categorias']
      ],
      gestion_ot: [
        ['Caratula OT', 'mdi-warehouse','Ot_encabezado_ot']
      ],
  }),

  watch: {
    group () {
      this.drawer = false
    },
  },
}
</script>
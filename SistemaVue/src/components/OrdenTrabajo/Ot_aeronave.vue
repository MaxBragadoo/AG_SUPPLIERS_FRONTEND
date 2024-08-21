<template>
  <v-data-table
    :headers="headers"
    :items="array_aeronaves"
    :search="search"
    :sort-by="[{ key: 'matricula', order: 'asc' }]"
    v-if="bMostrarModelo"
  >
    <template v-slot:top>
      <v-toolbar
        flat
      > 
      <v-toolbar-title>AERONAVES</v-toolbar-title>
      
        <v-divider
          class="mx-4"
          inset
          vertical
        ></v-divider>

        <!-- Campo de búsqueda -->
          <v-text-field
            v-model="search"
            label="Search"
            prepend-inner-icon="mdi-magnify"
            variant="outlined"
            hide-details
            single-line
          ></v-text-field>

          <v-spacer></v-spacer>  
          <v-btn
              class="ma-2"
              color="red"
              @click="crear()"
            >
              NUEVO
          </v-btn>
      

        <!-- Ventana de dialogo para aceptar borrar el registro -->
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to delete this item?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue-darken-1" variant="text" @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue-darken-1" variant="text" @click="deleteItemConfirm">OK</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>

      </v-toolbar>
    </template>
    

    <!-- Estos son los iconos es la ventana de dialogo para modificar el registro -->
    <template v-slot:[`item.actions`]="{ item }">
      <v-icon
        class="me-2"
        size="small"
        @click="editItem(item)"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        size="small"
        @click="deleteItem(item)"
      >
        mdi-delete
      </v-icon>
    </template>

    <!-- Botón de Reset -->
    <template v-slot:no-data>
      <v-btn
        color="primary"
        @click="mounted"
      >
        Reset
      </v-btn>
    </template>
  </v-data-table>

  <!-- Ventana de dialogo para modificar el registro Modelo Aeronave -->      
  <div>
    <v-layout row justify-center>
      <v-dialog v-model="dialog" max-width="700px">     
        <v-card>          
          <v-card-title>
            <span class="headline">{{ formTitle }}</span>
          </v-card-title>

          <v-sheet class="mx-auto" width="600" grid-list-md>
            <v-form >
              <v-text-field
                  v-model="id_aeronave"
                  label="Id Aeronave"
                  disabled="true"
              ></v-text-field>

              <v-text-field
                v-model="matricula"
                label="Matricula"
              ></v-text-field>

              <v-select
                  v-model="id_modelo"
                  label="Modelos"
                  :items="modelos_aeronave"
                 
                ></v-select>

              <v-text-field
                v-model="ns_planeador"
                label="No Serie"
              ></v-text-field>
           
              <v-text-field
                v-model= "capitan"
                label="Capitán"
              ></v-text-field>

              <v-text-field
                v-model="tel_capitan"
                label="Tel Capitán"
              ></v-text-field>

              <v-col>
                <v-btn @click="close">
                  Cancelar
                </v-btn>
                <v-btn @click="guardarAeronave"
                  class="me-4"
                  type="submit"
                >Guardar
                </v-btn>
              </v-col>              
            </v-form>
          </v-sheet>

        </v-card>
      </v-dialog>

      <v-dialog 
        v-model="dlgActDesCat"
        max-width="400"
        row justify-center
      >
        <v-btn @click = "dlgActDesCategoria=false">Cancelar</v-btn>
        <v-btn @clcik = "dlgActDesCategoria=false">Desactivar</v-btn>

      </v-dialog>
    </v-layout>
  </div>

</template>

<script>
import axios from 'axios'

  export default {
    data: () => ({
      dialog: false,
      bMostrarModelo:true,
      bMostrar: false,
      array_aeronaves: [],
      modelos_aeronave: [],
      search: '',
      id_modelo: 0,
      
      headers: [   
        { title: 'Matricula ', key: 'matricula' },
        { title: 'Modelo ', key: 'modelo' },
        { title: 'No Serie', key: 'ns_planeador' },
        { title: 'Capitán', key: 'capitan' },
        { title: 'Acciones', key: 'actions'}
      ],

      a_piston_jet :[
         'P','J'  
      ],

      editedItem: {
        matricula: '',
        fabricante: '',
        id_modelo: 0,
      },
      defaultItem: {
        matricula: '',
        fabricante: 0,
        id_modelo: 0,
      },
    }),
 

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'Nueva registro' : 'Editar registro'
      },
      computedDateFormatted(){
       
        return this.formatDate(this.editedItem.inicio_ops)
      }
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
      dialogDelete (val) {
        val || this.closeDelete()
      },
      dialogAeronave (val) {
        val || this.close()
      },
    },

    created () {
      this.mounted()
    },

    methods: {
      crear(){
        this.limpiar();
        this.selectModelos();
        this.dialog       = true
        this.editedIndex  = -1;
      },

      muestraDialogo(){
        this.dialogAeronave=true
      },

      rowClicked(row) { 
        this.toggleSelection(row.id);        
        console.log(row.id);
      },

      toggleSelection(keyID) {
        if (this.selectedRows.includes(keyID)) {
          this.selectedRows = this.selectedRows.filter(
            selectedKeyID => selectedKeyID !== keyID
          );
        } else {
          this.selectedRows.push(keyID);
          
        }
      },

      initialize () {

      },

      mounted () {
        axios
          .get('api/aeronave/listar')
          .then((response => {              
              this.array_aeronaves = response.data;
              //console.log(this.array_aeronaves);
            }));   
      },

      listar (){
     // alert("listar");     
        axios
        .get('api/aeronave/listar')
        .then((response => {
            this.array_aeronaves = response.data;
            //console.log(this.array_aeronaves);
        }));
      },

      editItem (item) {
        //alert("entro");
        this.id_modelo    = item.id_modelo;
        this.id_aeronave  = item.id_aeronave;
        this.matricula    = item.matricula;
        this.modelo       = item.modelo;
        this.ns_planeador = item.ns_planeador;
        this.unidad       = item.unidad;
       // this.fabricacion  = item.fabricante;
        this.capitan      = item.capitan;
        this.tel_capitan  = item.tel_capitan;
        this.cliente      = item.cliente;
        this.selectModelos();
        this.dialog       = true
        this.editedIndex  = 1;
      },

      close () {
        this.bMostrar =0;
        this.bMostrarModelo=true;
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },

      limpiar(){
        this.id_modelo    = 0;
        this.modelo       = '';
        this.id_aeronave  = 0;
        this.matricula    = '';
        this.ns_planeador = '';
        this.unidad       = '';
        this.capitan      = '';
        this.tel_capitan  = '';
        this.Cliente      = '';
        this.modelos_aeronave= [],
        this.editedIndex  = -1;
      },

      guardarAeronave(){       
         
          if ( this.editedIndex > -1){
            // alert("editar");
            let me = this;
            this.dialog = false;  
            //this.bMostrar =1;
           // this.bMostrarModelo=false;
           //console.log(me);
            axios.put ('api/aeronave/actualizar', {
              'id_modelo'   : me.id_modelo,
              'id_aeronave' : me.id_aeronave,
              'matricula'   : me.matricula,
              'ns_planeador': me.ns_planeador,
              'unidad'      : me.unidad,
              'capitan'     : me.capitan,
              'tel_capitan' : me.tel_capitan,
              'cliente'     : me.cliente,
              
            })
            .then(function(){
              //alert("entro")
              me.close();
              me.listar();
              me.limpiar();
            })
            .catch( error => {
              this.errorMessage = error.message; 
              console.error("Hay un error!", error);
            });

          } else {
            //Código para guardar    
            let me=this;
            axios.post('api/aeronave/crear',{
                'id_modelo'   : me.id_modelo,
                'matricula'   : me.matricula,
                'ns_planeador': me.ns_planeador,
                'unidad'      : me.unidad,
                'capitan'     : me.capitan,
                'tel_capitan' : me.tel_capitan,
                'cliente'     : me.cliente,
            }).then(function(){
                me.close();
                me.listar();
                me.limpiar();                      
            }).catch(function(error){
              this.errorMessage = error.message; 
              console.error("Hay un error!", error);
            });
        }        
      },

      selectModelos(){
          let me=this;
          //let header={"Authorization" : "Bearer " + this.$store.state.token};
          //let configuracion= {headers : header};
          var modelosArray=[];
          axios.get('api/ModeloControlador/Select')
            .then(function(response){
            modelosArray=response.data;
            modelosArray.map(function(x){
                  me.modelos_aeronave.push({title: x.modelo, value: x.id_modelo});
              });
              //console.log(me.modelos_aeronave)
          }).catch(function(error){
              console.log(error);
          });
      },

      /* 
      editItem2 (item2) {
        this.editedIndex = this.aeronaves.indexOf(item)
        this.editedItem = Object.assign({}, item)
       
      },

      deleteItem (item) {
        this.editedIndex = this.desserts.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogDelete = true
      },

      deleteItemConfirm () {
        this.desserts.splice(this.editedIndex, 1)
        this.closeDelete()
      },

      closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },
      
      save() 
      {
        if (this.editedIndex > -1) {
          Object.assign(this.aeronaves[this.editedIndex], this.editedItem)
          let me = this.aeronaves[this.editedIndex]
          this.bMostrar =1;
          this.bMostrarAeronaves=0;
          axios.put('api/aeronaves/actualizar',{
                        'id_aeronave': me.id_aeronave,
                        'matricula'  : me.matricula, 
                        'alterna'    : me.alterna,
                        'seccion'    : me.seccion,
                        'fabricante' : me.fabricante,
                        'modelo'     : me.modelo,
                        'n_s'        : me.n_s,
                        'anio'       : me.anio,
                        'id_oaci'    : me.id_oaci,                        
                        'inicio_ops' : me.inicio_ops,
                        'horas'      : me.horas,
                        'cupo'       : me.cupo,
                        'ciclos'     : me.ciclos,
                        'motores'    : me.motores,
                        'activa'     : me.activa,
                        'apu'        : me.apu,
                        'tarifas'    : me.tarifas,
                        'piloto3'    : me.piloto3,
                        'ot'         : me.ot,
                        'sobrecargo' : me.sobrecargo,   
                        'notas'      : me.notas, 

                    })
                    .then(response => this.id_aeronave = response.data.id)
                    .catch(error => {
                      this.errorMessage = error.message;
                      console.error("There was an error!", error);
                    });
          
        } else {
          this.desserts.push(this.editedItem)
        }
        this.close()
      },

      formatDate (date) {
        if (!date) return null
        const [year, month, day] = date.split('-')
        const [dia] = day.split('T')
        return `${dia}-${month}-${year}`
      },
      */
    },
  }
</script>

<style>
.rojo{
  background: "red";
}

.tr-blue { background-color:aliceblue; }
</style>
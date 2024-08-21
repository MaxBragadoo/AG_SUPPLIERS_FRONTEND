<template>
  <v-data-table
    :headers="headers"
    :items="modelos"
    :search="search"
    :sort-by="[{ key: 'matricula', order: 'asc' }]"
    v-if="bMostrarModelo"
  >
    <template v-slot:top>
      <v-toolbar
        flat
      > 
      <v-toolbar-title>MODELO AERONAVES</v-toolbar-title>
      
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

          <v-sheet class="mx-auto" width="600">
            <v-form >         
              <v-text-field
                v-model="modelo"
                label="Modelo"
                :rules="rules"
                 counter
                maxlength="8"
                hint="Este campo debe tener máximo 8 caracteres"
              ></v-text-field>

              <v-text-field
                v-model="descripcion"
                label="Descripción"
              ></v-text-field>
           
              <v-select
                v-model= "piston_jet"
                label="Piston/Jet"
                :items="a_piston_jet"
              ></v-select>

              <v-text-field
                v-model="fabricante"
                label="Fabricante"
              ></v-text-field>
              
              <v-combobox
                v-model="motor_1"
                label="Motor 1"                
                :items= "['S','N']"
              ></v-combobox>
              
              <v-combobox
                v-model="motor_2"
                label="Motor 2"                
                :items= "['S','N']"
              ></v-combobox>
              
              <v-combobox
                v-model="helice_1"
                label="Helice 1"                
                :items= "['S','N']"
              ></v-combobox>

              <v-combobox
                v-model="helice_2"
                label="Helice 2"                
                :items= "['S','N']"
              ></v-combobox>

              <v-combobox
                v-model="apu"
                label="APU"
                :items= "['S','N']"
              ></v-combobox>

              <v-col>
                <v-btn @click="close">
                  Cancelar
                </v-btn>
                <v-btn @click="guardarModelo"
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
        <v-btn @clcik = "dlgActDesCategoria=true">Desactivar</v-btn>

      </v-dialog>
    </v-layout>
  </div>

</template>

<script>
import axios from 'axios'

  export default {
    data: () => ({
      modelo: '',
      description: 'California is a state in the western United States',
      rules: [v => v.length <= 8 || 'Max 8 characters'],    
      dialog: false,
      bMostrarModelo:true,
      bMostrar: false,
      modelos: [],
      search: '',

      headers: [      
        { title: 'Id ', key: 'id_modelo' },
        { title: 'Modelo ', key: 'modelo' },
        { title: 'Descripción', key: 'descripcion' },
        { title: 'Piston/Jet', key: 'piston_jet' },
        { title: 'Acciones', key: 'actions'}
      ],

      a_piston_jet :[
        {title: 'PISTON', value: 'P'},
        {title: 'JET', value: 'J'}
      ],

      editedItem: {
        matricula: '',
        fabricante: '',
        modelo: '',
      },
      defaultItem: {
        matricula: '',
        fabricante: 0,
        modelo: '',
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
          .get('api/ModeloControlador/Listar')
          .then((response => {              
              this.modelos = response.data;
              //console.log("esta es una prueba");
            }));   
      },

      listar (){
        axios
        .get('api/ModeloControlador/Listar')
        .then((response => {
            this.modelos = response.data;
        }));
      },

      editItem (item) {
        //alert("entro");
        this.id_modelo    = item.id_modelo;
        this.modelo       = item.modelo;
        this.descripcion  = item.descripcion;
        this.piston_jet   = item.piston_jet;
        this.fabricante   = item.fabricante;
        this.motor_1      = item.motor_1;
        this.motor_2      = item.motor_2;
        this.helice_1     = item.helice_1;
        this.helice_2     = item.helice_2;
        this.apu          = item.apu;
        
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
        this.id_modelo    = '';
        this.modelo       = '';
        this.descripcion  = '';
        this.piston_jet   = '';
        this.fabricante   = '';
        this.motor_1      = '';
        this.motor_2      = '';
        this.helice_1     = '';
        this.helice_2     = '';
        this.apu          = '';
        this.editedIndex  = -1;
      },
      guardarModelo(){
          //alert("prueba");
          if ( this.editedIndex > -1){
            let me = this;
            this.dialog = false;  
            this.bMostrar =1;
            this.bMostrarModelo=false;
            axios.put ('api/modelocontrolador/actualizar', {
              'id_modelo'   : me.id_modelo,
              'modelo'      : me.modelo,
              'descripcion' : me.descripcion,
              'piston_jet'  : me.piston_jet,
              'motor_1'     : me.motor_1,
              'motor_2'     : me.motor_2,
              'helice_1'    : me.helice_1,
              'helice_2'    : me.helice_2,
              'apu'         : me.apu,
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
            alert("entro") 
            let me=this;
            axios.post('api/modelocontrolador/crear',{
                'modelo'      : me.modelo,
                'descripcion' : me.descripcion,
                'piston_jet'  : me.piston_jet,
                'motor_1'     : me.motor_1,
                'motor_2'     : me.motor_2,
                'helice_1'    : me.helice_1,
                'helice_2'    : me.helice_2,
                'apu'         : me.apu,
            }).then(function(){
                //console.log(response)
                me.close();
                me.listar();
                me.limpiar();                      
            }).catch(function(error){
              this.errorMessage = error.message; 
              console.error("Hay un error!", error);
            });
        }        
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
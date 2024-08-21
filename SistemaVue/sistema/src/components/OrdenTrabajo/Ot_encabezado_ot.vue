<template>
  <v-flex xs12 sm6 md4 >
    <!-- Ventana de dialogo para modificar el registro Reportes Programados -->   
    <v-card class="justify-center" width="1500">
      <v-data-table-virtual
        :headers="headers"
        :items="array_ots"
        :search="search"
        :sort-by="[{ key: 'id_ot', order: 'asc' }]"
        v-if="bMostrarOTs"
        >  
        <template v-slot:top>
          <v-toolbar
            flat
          > 
          <v-toolbar-title>CARATURA DE LA ORDEN DE TRABAJO</v-toolbar-title>          
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

      </v-data-table-virtual>
    </v-card>
    
    <!-- Ventana de dialogo para modificar el registro Reportes Programados -->   
    <v-card>
      <v-layout row justify-center>
        <v-template v-model="dialog" v-if ="bMostrarDetalle">  
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>          
            
            <v-sheet class="mx-auto" width="1500" >
              <v-form xs12 sm6 md4>
                <v-flex>
                  <!-- Renglon 1 -->   
                  <v-row>
                    <v-col>
                      <v-text-field
                          v-model="id_ot"
                          label="OT"
                          disabled="true"
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <v-select
                        v-model="id_aeronave"
                        label="Matricula"
                        :items="array_aeronaves"                       
                        v-on:update:model-value ="seleccionMatricula( id_aeronave )"
                      ></v-select>
                    </v-col>
                    <v-col>
                      <v-text-field
                          v-model="id_modelo"
                          label="Modelo"   
                          v-if="false"       
                      ></v-text-field>
                      <v-text-field
                          v-model="modelo"
                          label="Modelo"   
                          disabled="true"       
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <v-select
                        v-model="id_cliente"
                        label="Compañia"
                        :items="array_clientes"
                      ></v-select>
                    </v-col>
                  </v-row>  

                  <!-- Renglon 3 -->   
                  <v-row>                  
                    <v-col>
                      <v-text-field
                        v-model="contacto"
                        label="Contacto"
                      ></v-text-field>
                    </v-col> 
                    <v-col>
                      <v-text-field
                        v-model="telefono"
                        label="Teléfono"
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <v-text-field
                        v-model= "correo"
                        label="Correo"
                      ></v-text-field>
                    </v-col>               
                  </v-row>

                  <!-- Renglon 4 --> 
                  <v-row>
                    <v-col>
                      <v-text-field
                        v-model="direccion"
                        label="Dirección"
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <v-text-field
                        v-model= "ciudad"
                        label="Ciudad"
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <v-text-field
                          v-model="estado"
                          label="Estado"          
                      ></v-text-field>
                    </v-col>
                    <v-col>
                      <v-fab
                        class="ms-4"
                        icon="mdi-plus"
                        location="bottom end"
                        size="small"
                        @click="selectProgramadas( )"
                      ></v-fab>
                    </v-col>
                  </v-row> 
                  
                  <!-- Tabla del detalle --> 
                  <v-card width="100%">
                    <v-data-table-virtual
                        :headers="head_proragamdas_det"
                        :items=detalles
                        @click:row="rowClick"
                      >  
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
                    </v-data-table-virtual>
                  </v-card>

                  <!-- Renglon 6 - Botones  -->        
                  <v-row>
                    <v-col>
                      <v-btn @click="close">
                        Cancelar
                      </v-btn>        
                      <v-btn @click="guardarOts"
                        class="me-4"
                        type="submit"
                      >Guardar
                      </v-btn>
                    </v-col>  
                  </v-row>    

                </v-flex>                 
              </v-form>

            </v-sheet>

            <!-- Ventana de dialogo que permite editar una tarea programada-->  
            <v-dialog v-model="dialogDetProg" max-width="800px" v-if="bMostrarDetProg" dialog-margin="24px">              
              <v-flex>
                <v-card>
                  <v-card-title class="text-h5">Editar tarea programada</v-card-title>
                    <v-text-field
                      v-model="id_detalle"
                      label="Id detalle"
                      >
                    </v-text-field>
                    <v-text-field
                      v-model="id_programada"
                      label="Id programada"
                      >
                    </v-text-field>
                    <v-text-field
                      v-model="codigo"
                      label="Código"
                      >
                    </v-text-field>
                    <v-textarea
                      v-model="descripcion"
                      label="Descripción"
                      >
                    </v-textarea>
                    <v-textarea
                      v-model="accion"
                      label="Acción"
                      >
                    </v-textarea>  
                    
                    <!-- Renglon - Botones para Guardar o Cancelar tarea programada --> 
                    <v-card>
                      <v-btn @click="close">
                        Cancelar
                      </v-btn>        
                      <v-btn @click="guardarProg_en_Ot"
                        class="me-4"
                        type="submit"
                      >Guardar
                      </v-btn>
                    </v-card> 
                </v-card> 
              </v-flex>
            </v-dialog>                            
        </v-template> <!-- Este es un comentario-->

      </v-layout>
    </v-card>

    <!--Ventana que nos permite agregar una tarea programada al detalle de la OT-->
    <v-dialog max-width="1200" v-model="bMostrarProgramadas">
      <template v-slot:default="{ isActive }">
        <v-card title="Tareas programadas por modelo de aeronave">
          <v-data-table-virtual
            :headers="head_proragamdas"
            :items=array_programadas
            height="400"
            item-value="name"
          >
          
          <template v-slot:[`item.action_add`]="{ item }">
            <v-btn @click="agregarDetalle(item)">
              +
            </v-btn>
          </template>
          </v-data-table-virtual>

          <v-card-actions>
            <v-spacer></v-spacer>

            <v-btn
              text="Close Dialog"
              @click="isActive.value = false"
            ></v-btn>
          </v-card-actions>
        </v-card>
      </template>
    </v-dialog>

  </v-flex> 
</template>

<script>
import axios from 'axios'

  export default {
    data: () => ({
      bMostrarProgramadas: false,
      dialog: false,
      bMostrarOTs:true,
      bMostrarDetalle: false,
      bMostrarDetProg: false,
      array_aeronaves: [],
      array_ots: [],
      array_modelos: [],      
      array_clientes: [],
      array_programadas: [],
      detalles: [],
      search: '',
      id_ot       : 0,
      id_aeronave : 0,
      modelo      : '',
      id_cliente  : 0,
      contacto    :'',
      direccion   :'',
      ciudad      :'',
      estado      :'',
      correo      :'',
      telefono    :'',

      headers: [   
        { title: 'OT ', key: 'id_ot' },
        { title: 'Aeronave ', key: 'matricula' },
        { title: 'Modelo ', key: 'modelo' },
        { title: 'Contacto ', key: 'contacto' },
        { title: 'Teléfono  ', key: 'telefono' },
        { title: 'Correo', key: 'correo' },
        { title: 'Acciones', key: 'actions' }
      ],

      head_proragamdas:[
        { title: 'Id modelo ', key: 'id_modelo' },
        { title: 'Modelo ', key: 'modelo' },
        { title: 'Descripción ', key: 'descripcion' },
        { title: 'Acción ', key: 'accion' },
        { title: 'Agregar', key: 'action_add' },
      ],  

      head_proragamdas_det:[
      
        { title: 'Id programada ', key: 'id_programada' },
        { title: 'Modelo ', key: 'codigo' },
        { title: 'Descripción ', key: 'descripcion' },
        { title: 'Acción ', key: 'accion' },
        { title: 'Acciones ', key: 'actions_programadas' },
      ], 

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
      rowClick(item, row){
        console.log(row.item.id_programada);
        axios('api/partes')
        .then()
        

      },      
      crear(){
        this.limpiar();
        this.selectAeronaves();
        this.selectClientes();
        this.dialog          = true;
        this.bMostrarDetalle = true;
        this.bMostrarOTs     = false;
        this.editedIndex     = -1;
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

      seleccionMatricula(e){
        let me = this;  
        me.detalles =[];

        axios
        .get('api/aeronave/regresamodelo/' + e)
        .then((response => {
            this.array_ots = response.data;
            this.modelo = response.data[0].modelo;
        }));

      },

      agregarDetalle(data = []){       
          this.errorArticulo=null;
          if (this.encuentra(data['id_programada'])){
              this.errorArticulo="El tarea programada ya ha sido agregado."
              //alert("repetido");
          }
          else{
            //alert("agregar");
            this.detalles.push(
                {
                  'id_detalle'    : data['id_detalle'],
                  'id_programada' : data['id_programada'],
                  'codigo'        : data['codigo'],
                  'descripcion'   : data['descripcion'],
                }
            );
          }
      },

      encuentra(id){
          var sw=0;
          for(var i=0;i<this.detalles.length;i++){
              if (this.detalles[i].id_programada==id){
                  sw=1;
              }
          }
          return sw;
      },

      mounted () {
        axios
        .get('api/ordentrabajo/listar')
        .then((response => {              
            this.array_ots = response.data;
        }));   
      },

      listar (){  
        axios
        .get('api/ordentrabajo/listar')
        .then((response => {
            this.array_ots = response.data;
        }));
      },

      editItem (item) {   
        this.id_ot        = item.id_ot;     
        this.id_aeronave  = item.id_aeronave;
        this.id_modelo    = item.id_modelo;
        this.modelo       = item.modelo,
        this.id_cliente   = item.id_cliente;
        this.contacto     = item.contacto;
        this.telefono     = item.telefono;
        this.correo       = item.correo;
        this.direccion    = item.direccion;
        this.estado       = item.estado;
        this.ciudad       = item.ciudad; 
        this.listarDetalles(item.id_ot);      

        this.selectAeronaves();
        this.selectClientes();
        this.dialog           = true
        this.bMostrarDetalle  = true
        this.bMostrarOTs      = false
        this.editedIndex      = 1;
      },

      /*Edita las tareas programadas de la OT*/ 
      editItem_programadas (item) {   
        this.id_detalle      = item.id_detalle;  
        this.id_programada   = item.id_programada; 
        this.id_modelo       = item.id_modelo;     
        this.codigo          = item.codigo;
        this.descripcion     = item.descripcion;

        this.dialogDetProg   = true
        this.bMostrarDetProg = true
        this.bMostrarOTs     = false
        this.editedIndex     = 1;
      },

      guardarProg_en_Ot(){
        //alert("actualizar programadas " + this.id_programada)
        let me = this;
        axios.put ('api/ordentrabajo/actualizar_programada', {
          'id_detalle'    : me.id_detalle,
          'id_ot'         : me.id_ot,
          'id_programada' : me.id_programada,
          'codigo'        : me.codigo,
          'descripcion'   : me.descripcion,
          'accion'        : me.accion,
        })
        .then(function(){
          me.listar();
          me.close();             
          me.limpiar();
        })
        .catch( error => {
          this.errorMessage = error.message; 
          console.error("Hay un error!", error);
        });

      },

      close () {
        this.bMostrar       = 0;
        this.bMostrarOTs    = true;
        this.dialog         = false;
        this.bMostrarDetalle= false;
        this.bMostrarOTs    = true;
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },

      limpiar(){
        this.id_ot          = 0;
        this.id_aeronave    = 0;
        this.modelo         = '';
        this.id_modelo      = 0;
        this.id_cliente     = 0;
        this.contacto       = '';
        this.direccion      = '';
        this.ciudad         = '';
        this.estado         = '';
        this.telefono       = '';
        this.correo         = '';  
        this.detalles       = []; 
        this.array_clientes = [];
        this.array_aeronaves= [];
       
      },

      guardarOts(){     
          if ( this.editedIndex > -1){
            //alert("editar")
            let me = this;
            this.dialog = false;  
            axios.put ('api/ordentrabajo/actualizar/', {
              'id_ot'         : me.id_ot,
              'id_aeronave'   : me.id_aeronave,
              'modelo'        : me.id_modelo,
              'id_cliente'    : me.id_cliente,
              'contacto'      : me.contacto,  
              'telefono'      : me.telefono,
              'correo'        : me.correo,
              'direccion'     : me.direccion,
              'estado'        : me.estado,
              'ciudad'        : me.ciudad,               
              'detalles'      : me.detalles,
            })
            .then(function(){
              me.listar();
              me.close();             
              me.limpiar();
            })
            .catch( error => {
              this.errorMessage = error.message; 
              console.error("Hay un error!", error);
            });

          } else {
            //Código para guardar    
            let me=this;
            axios.post('api/ordentrabajo/crear',{
                'id_aeronave'   : me.id_aeronave,
                'id_modelo'     : me.id_modelo,
                'id_cliente'    : me.id_cliente,
                'contacto'      : me.contacto,  
                'telefono'      : me.telefono,
                'correo'        : me.correo,
                'direccion'     : me.direccion,
                'estado'        : me.estado,
                'ciudad'        : me.ciudad,  
                'detalles'      : me.detalles,

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
      
      /*Muestra las tareas programadas de la OT. Es decir es el detalle*/ 
      listarDetalles(id){
        let me=this;
        axios.get('api/ordentrabajo/ListarDetalles/' + id)
        .then(function(response){
            //console.log(response);
            me.detalles=response.data;
        }).catch(function(error){
            console.log(error);
        });
      },

      selectModelo(){  
        axios
        .get('api/modelocontrolador/select').then((response => {
            this.array_modelos = response.data;
            //console.log (response.data);
        }));
      },

      selectClientes(){
          let me=this;
          //let header={"Authorization" : "Bearer " + this.$store.state.token};
          //let configuracion= {headers : header};
          var clientesArray=[];
          axios.get('api/cliente/Select')
          .then(function(response){
            clientesArray=response.data;
            clientesArray.map(function(x){
                  me.array_clientes.push({title: x.nombre, value: x.id_cliente});
              });
              //console.log(me.array_clientes)
          }).catch(function(error){
              console.log(error);
          });
      },

      selectAeronaves(){        
          let me=this;
          //let header={"Authorization" : "Bearer " + this.$store.state.token};
          //let configuracion= {headers : header};
          var aeronavesArray=[];

          me.array_aeronaves=[];
          axios.get('api/Aeronave/Select')
          .then(function(response){
            
            aeronavesArray=response.data;
            aeronavesArray.map(function(x){
                  me.array_aeronaves.push({title: x.matricula, value: x.id_aeronave});
              });
            //console.log(me.array_aeronaves)
          }).catch(function(error){
              console.log(error);
          });
      },  

      selectProgramadas(){        
        this.bMostrarProgramadas =true;
        //let header={"Authorization" : "Bearer " + this.$store.state.token};
        //let configuracion= {headers : header};
        // alert(me.modelo)
        axios
          .get( 'api/programada/listar_x_modelo/' + this.modelo)
          .then((response => {
             this.array_programadas = response.data;
                //console.log(response.data);
          }));
      },    
    },
  }
</script>

<style>
.rojo{
  background: "red";
}

.tr-blue { background-color:aliceblue; }
</style>
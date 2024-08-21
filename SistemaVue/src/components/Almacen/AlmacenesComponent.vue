<template>
    <v-data-table :headers="headers" :items="almacenes" :search="search"
        :sort-by="[{ key: 'calories', order: 'asc' }]">
        <template v-slot:top>
            <v-toolbar flat>
                <v-toolbar-title>Almacenes</v-toolbar-title>
                <v-divider class="mx-2" inset vertical></v-divider>
                <v-spacer></v-spacer>
                <v-text-field class="text-xs-center" v-model="search" append-icon="mdi-magnify" label="Búsqueda"
                    single-line hide-details></v-text-field>
                <v-spacer></v-spacer>
                <v-dialog v-model="dialog" max-width="500px">
                    <template v-slot:activator="{ props }">
                        <v-btn class="mb-2" color="primary" dark v-bind="props">
                            Nuevo ALmacen
                        </v-btn>
                    </template>
                    <v-card>
                        <v-card-title>
                            <span class="text-h5">{{ formTitle }}</span>
                        </v-card-title>

                        <v-card-text>
                            <v-container>
                                <v-row>
                                    <v-col cols="6" md="6" sm="6">
                                        <v-number-input v-model="codigo"
                                        :rules="[v => !!v || 'El campo Codigo es Obligatorio']"
                                        required 
                                        label="Codigo"></v-number-input>
                                    </v-col>
                                    <v-col cols="12" md="12" sm="6">
                                        <v-text-field v-model="nombre"
                                            :counter="30"
                                            :rules="nameRules"
                                            required
                                            label="Nombre">
                                        </v-text-field>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" md="12" sm="6">
                                        <v-text-field v-model="ubicacion" label="Ubicación"></v-text-field>
                                    </v-col>
                                </v-row>
                                <!--Columna que solo se mostrara cuando no pase las validaciones el nombre-->
                                <v-row>
                                    <v-col cols="12" md="12" sm="6" v-show="valida">
                                        <div class="text-red" v-for="v in validaMensaje" :key="v" v-text="v">

                                        </div>
                                    </v-col>
                                </v-row>

                            </v-container>
                        </v-card-text>

                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="blue-darken-1" variant="text" @click="close">
                                Cancelar
                            </v-btn>
                            <v-btn color="blue-darken-1" variant="text" @click="guardar">
                                Guardar
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
                <!--Dialog creado para que se muestre cuando queremos activar o desactivar una categoria  -->
                <v-dialog v-model="adModal" elavation="2" max-width="290px">
                    <v-card>
                        <v-card-title class="haedline" v-if="adAccion==1">¿Activar Item?</v-card-title>
                        <v-card-title class="headline" v-if="adAccion==2">¿Desactivar Item?</v-card-title>
                        <v-card-text>
                            Estas apunto de
                            <span v-if="adAccion==2">Desactivar </span>
                            <span v-if="adAccion==1">Activar </span>
                            el item {{ adNombre }}
                        </v-card-text>
                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="green darken-1" flat="flat" @click="activarDesactivarCerrar">
                                Cancelar
                            </v-btn>
                            <v-btn v-if="adAccion==1" color="orange darken-4" flat="flat" @click="activar">
                                Activar
                            </v-btn>
                            <v-btn v-if="adAccion==2" color="orange darken-4" flat="flat" @click="desactivar">
                                Desactivar
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
                <!--Fin del Dialog para activar o desactivar una categoria-->
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
        
        <!-- <template v-slot:item.actions="{ item }">  -->
        <template v-slot:[`item.actions`]="{ item }">
            <v-icon class="me-2" size="small" @click="editItem(item)">
                mdi-pencil
            </v-icon>
            <template v-if="item.condicion">
                <v-icon size="small" @click="activarDesactivarMostrar(2,item)">
                    mdi-block-helper
                </v-icon>
            </template>
            <template v-else>
                <v-icon size="small" @click="activarDesactivarMostrar(1,item)">
                    mdi-check
                </v-icon>
            </template>
        </template>
         <!--CODIGO INSERTADO PARA PONER CHIP'S PARA PINTAR CAUNDO ESTA ACTIVADO Y DESACTIVADO EN LA COLUMNA ESTADO-->      
         <template  v-slot:[`item.condicion`]="{ value }">            
                <v-chip :color="getColor(value)">
                    {{getEtiqueta(value)}}
                </v-chip>           
        </template>
        <!--***********************FIN DEL CODIGO PARA PODER INSERTAR LOS COMPONENTS CHIPS PARA ACTIVADO Y DESACTIVADO -->
        <template v-slot:no-data>
            <v-btn color="primary" @click="initialize">
                Resetear
            </v-btn>
        </template>
    </v-data-table>
</template>

<script>

import axios from 'axios';

export default {
    data: () => ({
        dialog: false,
        dialogDelete: false,        
        headers: [
            { title: 'Opciones', value: 'actions', key: 'actions', sortable: false },
            //{ title: 'ID Almacen', value: 'idalmacen', key: 'idalmacen' },
            { title: 'Codigo', value: 'codigo', key: 'codigo' },
            { title: 'Nombre', value: 'nombre', key: 'nombre' },
            { title: 'Ubicacion', value: 'ubicacion', key: 'ubicacion', sortable: false },
            { title: 'Estado', value: 'condicion', key: 'condicion' },

        ],

        //Arreglo que nos muestra las categorias dentro de la tabla
        almacenes: [            
        ],
    
        //Variable para la busqueda dentro de la tabla
        search: '',

        //desserts: [],
        editedIndex: -1,

        idalmacen: '',
        codigo: 0,
        nombre: '',
        ubicacion: '',

        //Creacion de regla para el Campo nombre....
        nameRules: [
        v => !!v || 'El campo nombre es Requerido',
        v => (v && v.length >=3 && v.length <= 30) || 'El *Nombre* debe tener más de 3 caracteres y menos de 30 caracteres',
      ],


        //Variables que nos sirven para las validaciónes
        valida: 0,
        adModal:false,
        validaMensaje: [],       
        adAccion: 0,
        adNombre: '',
        adId: ''

    }),

    computed: {
        formTitle() {
            return this.editedIndex === -1 ? 'Nuevo Almacen' : 'Actualizar Almacen'
        },
    },

    watch: {
        dialog(val) {
            val || this.close()
        },
        dialogDelete(val) {
            val || this.closeDelete()
        },
    },

    created() {
        this.initialize();
        this.listar();
    },

    methods: {
        //Metodo para poder cargar los datos de la BD ALmacenes
        listar() {
            let me = this;
            axios.get('api/Almacenes/Listar')
            .then(function (response) {
               // console.log(response);
                me.almacenes = response.data;


            }).catch(function (error) {
                console.log(error);

            });
        },

        initialize() {
            this.almacenes = []
        },

        editItem(item) {
            this.idalmacen  = item.idalmacen;
            this.codigo     = item.codigo;
            this.nombre     = item.nombre;
            this.ubicacion  = item.ubicacion;
            this.editedIndex = 1;

            this.dialog = true
        },

        deleteItem(item) {
            this.editedIndex = this.desserts.indexOf(item)
            this.editedItem = Object.assign({}, item)
            this.dialogDelete = true
        },

        deleteItemConfirm() {
            this.desserts.splice(this.editedIndex, 1)
            this.closeDelete()
        },

        close() {
            this.dialog = false;
            this.limpiar();           
        },

        closeDelete() {
            this.dialogDelete = false
            this.$nextTick(() => {
                this.editedItem = Object.assign({}, this.defaultItem)
                this.editedIndex = -1
            })
        },
        //Metodo que nos sive para dejar los campos vacíos, una vez que vamos a gregar una nueva categoria
        limpiar(){
           
            this.idalmacen="";
            this.codigo="";
            this.nombre="";
            this.ubicacion="";
            this.editedIndex=-1;

        },
        //Metodo que nos sirve para poder guardar un nuevo registro..
        guardar() {

            //Antes de actualizar o crear un registro debemos de validar los campos que cumplan con la validacion de mas de 3 o menos de 50
            if(this.validar()){
                return;

            }

            if (this.editedIndex > -1) {
                //Codigo para Editar Una Categoria...
                let me = this;
                axios.put('api/Almacenes/Actualizar', {
                    'idalmacen': me.idalmacen,
                    'codigo': me.codigo,
                    'nombre': me.nombre,
                    'ubicacion': me.ubicacion,                    
                    
                }).then(function(){                   
                    me.close();
                    me.listar();
                    me.limpiar();
                    //console.log(response);                    

                }).catch(function(error){
                    console.log(error);
                });

            } else {
                //Codigo para Guardar un Almacen....
                let me = this;
                axios.post('api/Almacenes/Crear', {
                    'codigo': me.codigo,
                    'nombre': me.nombre,
                    'ubicacion': me.ubicacion,
                    
                    
                }).then(function(){                   
                    me.close();
                    me.listar();
                    me.limpiar();
                    //console.log(response);

                }).catch(function(error){
                    console.log(error);
                });
            }
        },
        //Metodo que nos sirve para las validaciones al agregar un Almacen.
        validar(){
            this.valida = 0;
            this.validaMensaje = [];

            if(this.nombre.length < 3 || this.nombre.length > 50){
                this.validaMensaje.push("El *Nombre* debe tener más de 3 caracteres y menos de 30 caracteres");
            }
            if (this.validaMensaje.length){
                this.valida = 1;
            }
            return this.valida;

            
        },
        //Metodo que nos va a funcioanr para poder cambiar el estatus de un Almacen por medio y mostrando un modal
        activarDesactivarMostrar(accion, item ){

            this.adModal=1;
            this.adNombre=item.nombre;
            this.adId=item.idalmacen;

            if(accion == 1){
                this.adAccion=1;
            }
            else if(accion==2){
                this.adAccion=2;
            }
            else{
                this.adModal=false;
            }
                        
        },
        activarDesactivarCerrar(){
            this.adModal=false;
        },
        //Metodo para activar una categoria en el modal
        activar(){
            let me=this;
            axios.put('api/Almacenes/Activar/'+ this.adId,{})
            .then(function(){
                me.adModal=false;
                me.adAccion = 0;
                me.adNombre="";
                me.adId;
                me.listar();
                //Esta linea solo la puse para que no me mostrara el error del response
                //console.log(response);
            }).catch(function (error){
                console.log(error);
            });

        },
        //Metodo para desactivar un Almacen en el modal
        desactivar(){
            let me=this;
            axios.put('api/Almacenes/Desactivar/'+ this.adId,{})
            .then(function(){
                me.adModal = false;
                me.adAccion = 0;
                me.adNombre="";
                me.adId;
                me.listar();
                //Esta linea solo la puse para que no me mostrara el error del response
                //console.log(response);
            }).catch(function (error){
                console.log(error);
            });

        },
        //Metodo para poder cambiar el color del v-chip en la columna Estado...
        getColor (condicion) {
            if (condicion == true) return 'green'        
            else return 'red'
        },
        //Metodo para poder cambiear el texto de Activo e Inactivo en el v-chip en la columna estado....
        getEtiqueta (condicion){
            if(condicion == true) return condicion = "Activo"
            else return 'Inactivo' 
        },
    },
}
</script>
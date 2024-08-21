<template>
    <v-data-table :headers="headers" :items="unidadMedidas" :search="search"
        :sort-by="[{ key: 'calories', order: 'asc' }]">
        <template v-slot:top>
            <v-toolbar flat>
                <v-toolbar-title>Maestro  de Unidad de Medidas</v-toolbar-title>
                <v-divider class="mx-2" inset vertical></v-divider>
                <v-spacer></v-spacer>
                <v-text-field class="text-xs-center" v-model="search" append-icon="mdi-magnify" label="Búsqueda"
                    single-line hide-details></v-text-field>
                <v-spacer></v-spacer>
                <v-dialog v-model="dialog" max-width="500px">
                    <template v-slot:activator="{ props }">
                        <v-btn class="mb-2" color="primary" dark v-bind="props">
                            Nueva Unidad de Medida
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
                                        <v-text-field v-model="codigo"
                                            :counter="5"
                                            :rules="nameRules"
                                            required
                                            label="Codigo">
                                        </v-text-field>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col cols="12" md="12" sm="6">
                                        <v-textarea v-model="descripcion"
                                            rows="2"
                                            :counter="256"
                                            required
                                             label="Descripcion">
                                        </v-textarea>
                                    </v-col>
                                </v-row>
                             <!--   <v-row>
                                    <v-col cols="6" md="6" sm="6">
                                        <v-date-input v-model="model" label="Fecha Alta:"></v-date-input>
                                    </v-col>
                                    <v-col cols="12" md="6" sm="6">
                                        <v-text-field v-model="f_baja" label="Fecha Baja:"></v-text-field>
                                    </v-col>
                                </v-row>  -->
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
                <!--Dialog creado para que se muestre cuando queremos activar o desactivar una Unidad de Medida-->
                <v-dialog v-model="adModal" elavation="2" max-width="290px">
                    <v-card>
                        <v-card-title class="haedline" v-if="adAccion==1">¿Activar Item?</v-card-title>
                        <v-card-title class="headline" v-if="adAccion==2">¿Desactivar Item?</v-card-title>
                        <v-card-text>
                            Estas apunto de
                            <span v-if="adAccion==1">Activar </span>
                            <span v-if="adAccion==2">Desactivar </span>
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
            <template v-if="item.estatus">
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
        <template  v-slot:[`item.estatus`]="{ value }">            
                <v-chip :color="getColor(value)" >
                    {{ getEtiqueta(value) }}
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
        model: null,
        dialog: false,
        dialogDelete: false,
        headers: [
            { title: 'Opciones', value: 'actions', key: 'actions', sortable: false },
            { title: 'ID U M', value: 'idum', key: 'idum' },
            { title: 'Codigo', value: 'codigo', key: 'codigo' },
            { title: 'Descripcion', value: 'descripcion', key: 'descripcion', sortable: false },
            { title: 'Estado', value: 'estatus', key: 'estatus' }
            //{ title: 'Fecha Alta', value:'f_alta', key: 'f_alta' },
            //{ title: 'Fecha Baja', value: 'f_baja', key: 'f_baja' }

        ],

        //Arreglo que nos muestra las Unidades de Medida dentro de la tabla
        unidadMedidas: [],
        //Variable para la busqueda dentro de la tabla
        search: '',

        //desserts: [],
        editedIndex: -1,

        idum: '',
        codigo: '',
        descripcion: '',
        f_alta: '',
        f_baja: '',

        //Creacion de regla para el Campo nombre....
        nameRules: [
        v => !!v || 'El campo Codigo es Requerido',
        v => (v && v.length >=3 && v.length <= 5) || 'El *Codigo* debe tener más de 3 caracteres y menos de 5 caracteres',
        ],


        //Variables que nos sirven para las validaciónes
        valida: 0,
        validaMensaje: [],
        adModal: false,
        adAccion: 0,
        adNombre: '',
        adId: '',

    }),

    computed: {
        formTitle() {
            return this.editedIndex === -1 ? 'Nueva Unidad de Medida' : 'Actualizar Unidad de Medida'
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
        //Metodo para poder cargar los datos de la BD Unidades de Medida
        listar() {
            let me = this;
            axios.get('api/UnidadMedidas/Listar').then(function (response) {

                //console.log(response);
                me.unidadMedidas = response.data;

            }).catch(function (error) {
                console.log(error);

            });
        },

        initialize() {
            this.unidadMedidas = []
        },

        editItem(item) {
            this.idum = item.idum;
            this.codigo      = item.codigo;
            this.descripcion = item.descripcion;
            this.f_alta      = item.f_alta;
            this.f_baja      = item.f_baja;
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
           
            this.idum       ="";
            this.codigo     ="";
            this.descripcion="";
            this.f_alta     ="";
            this.f_baja     ="";
            this.editedIndex=-1;
            
            

        },
        //Metodo que nos sirve para poder guardar un nuevo registro..
        guardar() {

            //Antes de actualizar o crear un registro debemos de validar los campos que cumplan con la validacion de mas de 3 o menos de 50
            if(this.validar()){
                return;

            }

            if (this.editedIndex > -1) {
                //Codigo para Editar Una Unidad de Medida...
                let me = this;
                axios.put('api/UnidadMedidas/Actualizar', {
                    'idum': me.idum,
                    'codigo': me.codigo,
                    'descripcion': me.descripcion,
                    'f_alta': me.f_alta,
                    'f_baja': me.f_baja
                    
                    
                }).then(function(response){                   
                    me.close();
                    me.listar();
                    me.limpiar();

                    console.log(response);
                    

                }).catch(function(error){
                    console.log(error);
                });

            } else {
                //Codigo para Guardar una Unidad de Medida....
                let me = this;
                axios.post('api/UnidadMedidas/Crear', {
                    'codigo': me.codigo,                    
                    'descripcion': me.descripcion,
                    'estatus': true
                    //'f_alta': me.f_alta,
                    //'f_baja': me.f_baja                    
                }).then(function(response){                   
                    me.close();
                    me.listar();
                    me.limpiar();

                    console.log(response);
                    

                }).catch(function(error){
                    console.log(error);
                });
            }
        },
        //Metodo que nos sirve para las validaciones al agregar una Undad de Medida.
        validar(){
            this.valida = 0;
            this.validaMensaje = [];

            if(this.codigo.length < 3 || this.codigo.length > 50){
                this.validaMensaje.push("El *Nombre* debe tener más de 3 caracteres y menos de 50 caracteres");
            }
            if (this.validaMensaje.length){
                this.valida = 1;
            }
            return this.valida;

            
        },
        //Metodo que nos va a funcioanr para poder cambiar el estatus de una Categoria por medio y mostrando un modal
        activarDesactivarMostrar(accion, item ){

            this.adModal=1;
            this.adNombre=item.codigo;
            this.adId=item.idum;

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
            axios.put('api/UnidadMedidas/Activar/'+ this.adId,{}).then(function(response){
                me.adModal=false;
                me.adAccion=0;
                me.adNombre="";
                me.adId;
                me.listar();
                //Esta linea solo la puse para qu eno me mostrara el error del response
                console.log(response);
            }).catch(function (error){
                console.log(error);
            });

        },
        //Metodo para desactivar una categoria en el modal
        desactivar(){
            let me=this;
            axios.put('api/UnidadMedidas/Desactivar/'+ this.adId,{}).then(function(response){
                me.adModal=false;
                me.adAccion=0;
                me.adNombre="";
                me.adId;
                me.listar();
                //Esta linea solo la puse para qu eno me mostrara el error del response
                console.log(response);
            }).catch(function (error){
                console.log(error);
            });

        },
         //Metodo para poder cambiar el color del v-chip en la columna Estado...
         getColor (estatus) {
            if (estatus == true) return 'green'        
            else return 'red'
        },
        //Metodo para poder cambiear el texto de Activo e Inactivo en el v-chip en la columna estado....
        getEtiqueta (estatus){
            if(estatus == true) return estatus = "Activo"
            else return 'Inactivo' 
        },
    },
}
</script>
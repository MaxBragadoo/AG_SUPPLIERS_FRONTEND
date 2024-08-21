<template>
    <v-data-table :headers="headers" :items="articulos" :search="search"
        :sort-by="[{ key: 'calories', order: 'asc' }]">      
        <template v-slot:top>
            <v-toolbar flat>
                <v-toolbar-title>Listado de Articulos</v-toolbar-title>
                <v-divider class="mx-2" inset vertical></v-divider>

                <v-spacer></v-spacer>

                <v-text-field class="text-xs-center" v-model="search" append-icon="mdi-magnify" label="Búsqueda"
                    single-line hide-details></v-text-field>
                <v-spacer></v-spacer>
                <v-dialog v-model="dialog" max-width="1200px">
                    <template v-slot:activator="{ props }">
                        <v-btn class="mb-2" color="primary" dark v-bind="props">
                            Nuevo
                        </v-btn>
                    </template>
                    <v-card>                       
                        <v-card-title>                            
                            <span class="text-h5">{{ formTitle }}</span>                        
                        </v-card-title>
                  
                        <v-divider :thickness="2"></v-divider>
                        
                        <v-card-text>
                            <v-container>
                                <v-row>
                                    <v-col cols="12" md="4" sm="4">
                                        <v-text-field v-model="codigo" label="Código"></v-text-field>                                       
                                    </v-col>
                                     <v-col cols="12" md="4" sm="4">
                                        <v-text-field v-model="numero_serie" label="Numero de Serie"></v-text-field>
                                    </v-col>
                                    <v-col cols="12" md="4" sm="4">
                                        <v-select v-model="idcategoria" 
                                                :items="categorias"
                                                :rules="[v => !!v || 'Seleccione una Categoria']" 
                                                label="Categorias"
                                                required>
                                        </v-select>
                                    </v-col>                                    
                                </v-row>                            
                                <v-row>
                                    <v-col cols="6" md="3" sm="3">
                                        <v-select v-model="idum" 
                                                :items="unidadMedidas"
                                                :rules="[v => !!v || 'Seleccione una U/M']"
                                                label="U/M"
                                                rfequired>
                                        </v-select>
                                    </v-col>
                                    <v-col cols="6" md="3" sm="3">
                                        <v-select v-model="idalmacen" 
                                                    :items="almacenes" 
                                                    :rules="[v => !!v || 'Seleccione un Almacen']"
                                                    label="Almacen"
                                                    required>
                                        </v-select>
                                    </v-col>
                                    <v-col cols="6" md="3" sm="3">
                                        <v-select v-model="idubicacion" 
                                                    :items="ubicaciones" 
                                                    :rules="[v => !!v || 'Seleccione una Ubicación']"
                                                    label="Ubicacion"
                                                    required>
                                        </v-select>
                                    </v-col>
                                    <v-col cols="6" md="3" sm="3">                                                                         
                                        <v-text-field v-model="localizacion" label="Localizcion"></v-text-field>
                                    </v-col>                                    
                                </v-row>
                                <v-row>
                                    <v-col md="3" sm="3">
                                        <v-text-field type="number" v-model="stock" label="Stock"></v-text-field>                                        
                                    </v-col>                                                          
                                  
                                    <v-col md="3" sm="3">
                                        <v-text-field v-model="precio_venta" label="Precio de Venta"></v-text-field>
                                    </v-col>

                                    <v-col md="3" sm="3">
                                        <v-text-field v-model="maximo" label="Máximo"></v-text-field>
                                    </v-col>
                                    <v-col md="3" sm="3">
                                        <v-text-field v-model="minimo" label="Mínimo"></v-text-field>
                                    </v-col>                                                                        
                                </v-row>
                                
                                <v-row>
                                    <v-col cols="12" md="4" sm="4">
                                        <v-select v-model="idproveedor" 
                                            :items="proveedores" 
                                            :rules="[v => !!v || 'Seleccione un Proveedor']"
                                            label="Proveedores"
                                            required>
                                        </v-select>
                                    </v-col>   
                                    <!-- <v-col>
                                        <v-text-field 
                                            v-model="formattedDate" 
                                            label="ultima"
                                            >
                                        </v-text-field>
                                    </v-col>  
                                    <v-col>
                                        <v-date-input v-model="ultima_salida" 
                                            label="Date input"
                                        ></v-date-input>
                                    </v-col>  -->                                                                                                  
                                </v-row>

                                <v-row>
                                    <v-col cols="12" md="12" sm="12">
                                        <v-textarea v-model="descripcion" rows="2" auto-grow label="Descripcion"></v-textarea>                                                                                
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

                <!--Dialog creado para que se muestre cuando queremos activar o desactivar una categoria-->
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

        <template v-slot:[`item.acciones`]="{ item }">
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
            //{ title: 'Opciones', value: 'acciones', key: 'accciones', sortable: false },
            //{ title: 'ID Articulo', value: 'idarticulo', key: 'idarticulo' },
            //{ title: 'ID Categoria', value:'idcategoria', key: 'idcategoria' },
            { title: 'Codigo', value: 'codigo', key: 'codigo' },
            { title: 'Numero Serie', value: 'numero_serie', key: 'numero_serie' },
            { title: 'Categoria', value: 'categoria', key: 'categoria' },
            //{ title: 'Descripción', value: 'descripcion', key: 'descripcion' },
            { title: 'Unidad Medida:', value: 'unidadMedida', key: 'unidadMedida'},
            { title: 'Almacen', value: 'almacen', key: 'almacen' },
            { title: 'Ubicacion', value: 'ubicacion', key: 'ubicacion'},
            { title: 'Localización', value: 'localizacion', key: 'localizacion' },            
            { title: 'Proveedor', value: 'proveedor', key: 'proveedor'},            
            { title: 'Precio Venta', value: 'precio_venta', key: 'precio_venta' },
            { title: 'Stock', value: 'stock', key: 'stock' },          
            { title: 'Máximo', value: 'maximo', key: 'maximo' },
            { title: 'Mínimo', value: 'minimo', key: 'minimo' },
            //{ title: 'Ult entrada', value: 'ultima_salida', key: 'ultima_entrada',  type:'int'},
            //{ title: 'Ult salida', value: 'ultima_salida', key: 'ultima_salida' }

        ],

        //Arreglo que nos muestra los Articulos dentro de la tabla principal
        articulos: [],
        //Variable para la busqueda dentro de la tabla
        search: '',

        desserts: [],
        editedIndex: -1,

        idarticulo: '',
        idcategoria: '',
        idum: '',
        idalmacen: '',
        idproveedor:'',
        idubicacion: '',
        codigo: '',
        numero_serie: '',
        precio_venta: '',        
        localizacion: '',
        stock: 0,
        cantidad: 0,
        descripcion: '',
        entrada: 0,
        salida: 0,

        
        categorias:[          
        ],
        unidadMedidas:[
        ],
        almacenes:[            
        ],
        proveedores:[            
        ],
        ubicaciones:[            
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
            return this.editedIndex === -1 ? 'Nuevo Articulo' : 'Actualizar Articulo'
        },
        
        formatoFecha(sqlFecha) {
            if (!sqlFecha) return null
            const [fecha, hora] = sqlFecha.split('T');
            const [year, month, day] = fecha.split('-');
            console.log(hora);
            return `${year}/${month}/${day}`;
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
        this.selectAlmacenes();
        this.selectCategoria();
        this.selectUnidadMedida();
        this.selectProveedores();
        this.selectUbicaciones(); 
        this.formattedDate();      
    },

    methods: {
        formattedDate() {
            //alert("entro");
            if (!this.ultima_entrada) return null
            const [fecha, hora] = this.ultima_entrada.split('T');
            const [year, month, day] = fecha.split('-');
            console.log(hora);
            return `${year}/${month}/${day}`;
        },
        //Metodos para poder trabajar con los datos de la BD de Articulos
        //Metodo para mostrar la lista de articulos 
        listar() {
            let me = this;
            axios.get('api/articulos/Listar').then(function (response) {

                console.log(response.data);
                me.articulos = response.data;
            }).catch(function (error) {
                console.log(error);

            });
        },

        initialize() {
            this.articulos = []
        },
        //Metodo que nos ayudara a llenar el select de almacenes al crear o actualizar un articulo
        selectAlmacenes(){
            let me = this;
            var almacenesArray=[];
            axios.get('api/Almacenes/Select').then(function(response){
                almacenesArray = response.data;
                almacenesArray.map(function(x){
                    me.almacenes.push({title: x.nombre, value: x.idalmacen})

                });             

            }).catch(function (error){
                console.log(error);
            });

        },
         //Metodo que nos ayudara a llenar el select de Proveedores al crear o actualizar un articulo
         selectProveedores(){
            let me = this;
            var proveedoresArray=[];
            axios.get('api/Proveedores/Select').then(function(response){
                proveedoresArray = response.data;
                proveedoresArray.map(function(x){
                    me.proveedores.push({title: x.nombre, value: x.idproveedor})

                });             

            }).catch(function (error){
                console.log(error);
            });

        },

        //Metodo que nos ayudara a llenar el elect de categorias al crear o actualizar un articulo
        selectCategoria(){
            let me = this;
            var categoriasArray=[];
            axios.get('api/Categorias/Select').then(function(response){
                categoriasArray = response.data;
                categoriasArray.map(function(x){
                    me.categorias.push({title: x.nombre, value: x.idcategoria})

                });             

            }).catch(function (error){
                console.log(error);
            });

        },
        //Metodo que nos ayuda a llenar el select de Ubicaciones al crear o actualizar un articulo..
        selectUbicaciones(){
            let me = this;
            var ubicacionesArray=[];
            axios.get('api/Ubicaciones/Select').then(function(response){
                ubicacionesArray = response.data;
                ubicacionesArray.map(function(x){
                    me.ubicaciones.push({title: x.nombre, value: x.idubicacion})
                });
            }).catch(function (error){
                console.log(error);
            });
        },

        //Metodo que nos ayudara a llenar el select de unidades de medida al crear o actualizar un articulo
        selectUnidadMedida(){
            let me = this;
            var uMedidasArray=[];
            axios.get('api/UnidadMedidas/Select').then(function(response){
                uMedidasArray = response.data;
                uMedidasArray.map(function(x){
                    me.unidadMedidas.push({title: x.descripcion, value: x.idum})

                });
            }).catch(function (error){
                console.log(error);
            });

        },
         
        editItem(item) {
            this.idarticulo     = item.idarticulo;
            this.idalmacen      = item.idalmacen;
            this.idcategoria    = item.idcategoria;            
            this.idum           = item.idum;
            this.idproveedor    = item.idproveedor;
            this.idubicacion    = item.idubicacion;           
            this.codigo         = item.codigo;
            this.numero_serie   = item.numero_serie;
            this.descripcion    = item.descripcion;                       
            this.localizacion   = item.localizacion;
            this.precio_venta   = item.precio_venta;
            this.cantidad       = item.cantidad;            
            this.stock          = item.stock;                       
            this.maximo         = item.maximo;
            this.minimo         = item.minimo;
            this.ultima_entrada = item.ultima_entrada;
            this.ultima_salida  = item.ultima_salida
            
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
           
            this.idcategoria            = "";
            this.idalmacen              = "";
            this.idum                   = "";
            this.idproveedor            = "";
            this.idubicacion            = "";            
            this.codigo                 = "";
            this.numero_serie           = "";
            this.precio_venta           = "";
            this.idubicacion            = "";
            this.localizacion           = "";
            this.stock                  = "";
            this.cantidad               = "";            
            this.descripcion            = "";
            this.maximo                 = "";
            this.minimo                 = "";
            this.ultima_entrada         = "";
            this.ultima_salida          = "";

            
            this.editedIndex=-1;

        },
        //Metodo que nos sirve para poder guardar un nuevo registro..
       
        guardar() {
            if (this.editedIndex > -1) {
                //Codigo para Editar / Actualizar Un Articulo...
                let me = this;
                //alert(me.idarticulo);
                axios.put('api/Articulos/Actualizar', {
                    
                    'idarticulo'    :me.idarticulo,
                    'idcategoria'   :me.idcategoria,
                    'idalmacen'     :me.idalmacen,
                    'idum'          :me.idum,
                    'idproveedor'   :me.idproveedor,
                    'idubicacion'   :me.idubicacion,
                    'codigo'        :me.codigo,
                    'numero_serie'  :me.numero_serie,
                    'precio_venta'  :me.precio_venta,                    
                    'localizacion'  :me.localizacion,
                    'stock'         :me.stock,
                    'maximo'        :me.maximo,
                    'minimo'        :me.minimo,
                    'descripcion'   :me.descripcion,
                    'ultima_entrada':me.ultima_entrada,
                    'ultima_salida' :me.ultima_salida,                 
                    
                }).then(function(response){                   
                    me.close();
                    me.listar();
                    me.limpiar();
                    console.log(response); 
                }).catch(function(error){
                    console.log(error);
                    //alert('GUARDAR: '+error)
                });

            } else {
                //Codigo para Guardar una Categoria....
                
                let me = this;
                axios.post('api/Articulos/Crear', {
                    'idcategoria'   :me.idcategoria,
                    'idalmacen'     :me.idalmacen,
                    'idum'          :me.idum,
                    'idproveedor'   :me.idproveedor,
                    'idubicacion'   :me.idubicacion,
                    'codigo'        :me.codigo,
                    'numero_serie'  :me.numero_serie,
                    'precio_venta'  :me.precio_venta,                    
                    'localizacion'  :me.localizacion,
                    'stock'         :me.stock,
                    'maximo'        :me.maximo,
                    'minimo'        :me.minimo,
                    'descripcion'   :me.descripcion,
                    'ultima_entrada':me.ultima_entrada,
                    'ultima_salida' :me.ultima_salida,    
                    
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
        //Metodo que nos sirve para las validaciones al agregar una categoria.
        validar(){
            this.valida = 0;
            this.validaMensaje = [];

            if(this.nombre.length < 3 || this.nombre.length>50){
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
            this.adNombre=item.nombre;
            this.adId=item.idcategoria;

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
        //Metodo para activar un Articulo en el modal
        activar(){
            let me=this;
            axios.put('api/Articulos/Activar/'+ this.adId,{}).then(function(response){
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
            axios.put('api/Articulos/Desactivar/'+ this.adId,{}).then(function(response){
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
    },
}
</script>
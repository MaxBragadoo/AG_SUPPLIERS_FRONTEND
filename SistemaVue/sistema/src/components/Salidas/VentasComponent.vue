<template>
    <v-layout align-start>
      <v-flex>
        <v-toolbar flat color="white">
          
          <v-toolbar-title class="font-weight-bold">Salidas OT</v-toolbar-title>
  
          <v-divider class="mx-2" inset vertical></v-divider>
          <v-spacer></v-spacer>
  
         <!-- <v-text-field
            v-if="verNuevo == 0"
            class="text-xs-center"
            v-model="search"
            append-icon="magnify"
            label="Búsqueda"
            single-line
            hide-details
          ></v-text-field>-->
  
          <v-text-field v-if="verNuevo == 0" class="text-xs-center" v-model="search" append-icon="mdi-magnify" label="Búsqueda"
          single-line hide-details></v-text-field>
  
         <!-- <v-btn v-if="verNuevo == 0" @click="listar()" color="primary" dark class="mb-2">Buscar</v-btn>-->
          <v-spacer></v-spacer>
          <v-btn v-if="verNuevo == 0" @click="mostrarNuevo" color="primary" dark class="mb-2">Nueva Salida OT</v-btn>
          <v-spacer></v-spacer>
  
          <v-dialog v-model="verArticulos" max-width="1000px">
            <v-card>
              <v-card-title>
                <span class="headline">Seleccione un artículo</span>
              </v-card-title>
              <v-card-text>
                <v-container grid-list-md>
                  
                  <v-text-field
                        append-icon="search"
                        class="text-xs-center"
                        v-model="texto"
                        label="Ingrese artículo a buscar"
                        @input="selectArticulo()" 
                      ></v-text-field>
  
                  <v-layout wrap>
  
                 
  
                    <v-flex xs12 sm12 md12 lg12 xl12>   
  
                    </v-flex>
  
                    <v-data-table v-model="idarticulo" 
                        :items="temporalArticulos"
                        :item-text="codigo"
                        :item-value="idarticulo">
  
                        <template v-slot:item="props">
                          <tr>
                            <td class="justify-center layout px-0">
                              <v-btn
                                icon
                                small
                                @click="agregarDetalle(props.item)"
                                color="primary">
                                <v-icon>mdi-plus</v-icon>
                              </v-btn>
                            </td>
                            <td>{{ props.item.idarticulo }}</td>
                            <td>{{ props.item.codigo }}</td>
                            <td>{{ props.item.idcategoria }}</td>
                            <td>{{ props.item.descripcion }}</td>
                            <td>{{ props.item.stock }}</td>
                            <td>{{ props.item.precio_venta }}</td>
                          </tr>
                        </template>
                        <template v-slot:no-data>
                          <h3>No hay artículos para mostrar.</h3>
                        </template>
                        
                    </v-data-table>
  
                  </v-layout>
                </v-container>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn @click="ocultarArticulos()" color="blue darken" flat>Cancelar</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
  
         <!--Dialog creado para que se muestre cuando queremos eliminar un item del detalle de ventas -->
                <v-dialog v-model="adModal" elavation="2" max-width="290px">
                    <v-card>
                        <v-card-title class="haedline" v-if="adAccion==1">Eliminar Item?</v-card-title>
                        <v-card-text>
                            Estas apunto de
                            <span> eliminar </span>
                            el item {{ adNombre }}
                        </v-card-text>

                        <v-card-actions>
                            <v-spacer></v-spacer>
                            <v-btn color="green darken-1" flat="flat" @click="activarDesactivarCerrar">
                                Cancelar
                            </v-btn>
                            <v-btn color="orange darken-4" flat="flat" @click="eliminarDetalle(detalles,adNombre)">
                                Eliminar
                            </v-btn>
                        </v-card-actions>
                    </v-card>
                </v-dialog>
                <!--Fin del Dialog para eliminar un item del detalle de venta-->


        </v-toolbar>
  
        <div style="max-height: 1000px; overflow-y: auto;">
          <v-data-table :headers="headers" :items="ventas" :search="search" class="elevation-1" v-if="verNuevo == 0">
            <template v-slot:item="props">
              <tr>
                <td class="text-center">
                  <v-icon small class="me-2" @click="verDetalles(props.item)">
                   mdi-pencil
                  </v-icon>
              
                    <v-icon small @click="activarDesactivarMostrar(2, props.item)"> 
                      mdi-block-helper
                    </v-icon>
                    
                    <!--<v-icon
                      size="small"
                      @click="deleteItem(props)"
                    >
                      mdi-delete
                    </v-icon>-->
               
                </td>
           
                <td>{{ props.item.tipo_comprobante }}</td>
                <td>{{ props.item.tipo_movimiento }}</td>
                <td>{{ props.item.serie_comprobante }}</td>
                <td>{{ props.item.num_comprobante }}</td>
                <!--<td>{{ props.item.fecha_hora }}</td>-->
                <td>{{ props.item.impuesto }}</td>
                <td>{{ props.item.total }}</td>
                <td>
                  <div v-if="props.item.estado == 'Aceptado'">
                    <span class="blue--text">Aceptado</span>
                  </div>
                  <div v-else>
                    <span class="red--text">{{ props.item.estado }}</span>
                  </div>
                </td>
              </tr>
            </template>
            <template v-slot:no-data>
              <v-btn color="primary" @click="listar">Resetear</v-btn>
            </template>
          </v-data-table>
        </div>
  
        <v-container v-if="verNuevo" class="pa-4 white">
          <v-row>
            <v-col cols="12">
           
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field v-model="idventa" label="Folio Venta" disabled></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-select v-model="tipo_movimiento" :items="tipo_movimientos" label="Tipo Movimiento"></v-select>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-select v-model="tipo_comprobante" :items="comprobantes" label="Tipo Comprobante"></v-select>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field v-model="serie_comprobante" label="Serie Comprobante"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field v-model="num_comprobante" label="Número Comprobante"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
             
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-text-field type="number" v-model="impuesto" label="Impuesto"></v-text-field>
            </v-col>
            <v-col cols="12" sm="6" md="4">
              <v-btn @click="mostrarArticulos" small fab dark color="teal">
                <v-icon>mdi-format-list-bulleted</v-icon>
              </v-btn>
            </v-col>
            <v-col v-if="errorArticulo" cols="12" sm="6" md="4">
              <!--div class="red--text">{{ errorArticulo }}</div> -->
            </v-col>
            <v-col cols="12">
              <v-data-table :headers="cabeceraDetalles" :items="detalles" :hide-default-footer="true" class="elevation-1">
                <template v-slot:item="{ item }">
                  <tr>
                    <td class="text-center">
                      <v-icon small @click="preguntarBorrarDetalle(detalles, item)">mdi-delete</v-icon>
                    </td>
                    <td>{{ item.idarticulo }}</td>
                    <td>{{ item.codigo }}</td>
                    <td>
                      <input type="text" v-model="item.descripcion" :readonly = "!item.editable">
                    </td>
                    <td>
                      <v-text-field type="number" v-model="item.cantidad" @input="actualizarTotal" outlined dense></v-text-field>
                    </td>
                    <td>$
                      <input type="number" v-model="item.precio" @input="actualizarTotal" :readonly = "!item.editable">
                    </td>
                    <td>$ {{ item.cantidad * item.precio }}</td>
                  </tr>
                </template>
                <template v-slot:no-data>
                  <h3>No hay artículos agregados al detalle.</h3>
                </template>
              </v-data-table>
              <div class="text-right">
                <strong>Total Parcial: </strong>$ {{ totalParcial }}
              </div>
              <div class="text-right">
                <strong>Total Impuesto: </strong>$ {{ totalImpuesto }}
              </div>
              <div class="text-right">
                <strong>Total Neto: </strong>$ {{ total }}
              </div>
            </v-col>
            <v-col cols="12">
              <div v-for="(v, index) in validaMensaje" :key="index" class="red--text">{{ v }}</div>
            </v-col>
            <v-col cols="12" class="d-flex justify-left">
              <v-btn @click="ocultarNuevo" color="blue-darken-1" text class="mr-4">Cancelar</v-btn>
              <v-btn v-if="verDet === 0" @click="guardar" color="success">Guardar</v-btn>
              <v-btn v-if="verDet === 1" @click="actualizar" color="success">Actualizar</v-btn>
  
            </v-col>
          </v-row>
        </v-container>
      </v-flex>
    </v-layout>
  
    <v-snackbar v-model="snackbar.show" :timeout="snackbar.timeout" color="error">
      {{ snackbar.text }}
      <v-btn color="red" text @click="snackbar.show = false">Cerrar</v-btn>
    </v-snackbar>
  
  
  </template>
  <script>
  
  import { ref, computed, watch, onMounted } from 'vue';
  import axios from 'axios';
  export default {    
    setup() {   
      const temporalProveedores = ref([]); 
      const temporalArticulos = ref([]);
      const dialog = ref(false);
      //const dialogDelete = ref(false);
      //const dialogNew = ref(false);
      const ventas = ref([]);
      const headers = ref([
        { title: 'Opciones', key: 'opciones' },
        { title: 'Comprobante',key:'comprobante'},
        { title: 'Tipo Movimiento', key: 'tipo_movimiento' },
        { title: 'Serie Comprobante', key: 'serie_comprobante' },
        { title: 'Número Comprobante', key: 'numero_comprobante' },
        //{ title: 'Fecha', key: 'fecha' },
        { title: 'Impuesto', key: 'impuesto' },
        { title: 'Total', key: 'total' },
        { title: 'Estado', key: 'estado' },
      ]);
      const cabeceraDetalles = ref([
        { title: 'Borrar', key: 'borrar' },
        { title: 'IdArticulo', key: 'idarticulo' },
        { title: 'Código Artículo', key: 'codigo' },
        { title: 'Descripción', key: 'descripcion' },
        { title: 'Cantidad', key: 'cantidad' },
        { title: 'Precio', key: 'precio' },
        { title: 'Subtotal', key: 'subtotal' },
      ]);
  
  
      const id = ref('');
      const search = ref ('');
      const idarticulo = ref('');
      const tipo_comprobante = ref('');
      const tipo_movimiento = ref('');
      //const idusuario = ref('');
      const comprobantes = ref(['PEDIMIENTO', 'FACTURA', 'BOLETA', 'TICKET', 'GUIA']);
      const tipo_movimientos = ref([
        {title:'Salidas', value: 'S01'}, 
      ]);
      const serie_comprobante = ref('');
      const num_comprobante = ref('');
      const idventa = ref(0);
      const impuesto = ref(16);
      const detalle = [];
      const codigo = ref('');
      const verNuevo = ref(false);
  
      const cabeceraArticulos = ref([
        { title: 'Seleccionar', key: 'seleccionar' },
        { title: 'Código Artículo', key: 'codigo' },
        { title: 'ID Categoría', key: 'idcategoria' },
        { title: 'Descripción', key: 'descripcion' },
        { title: 'Stock', key: 'stock' },
        { title: 'Precio Venta', key: 'precio_venta' },
      ]);
      const articulos = ref([]);
      const articulosOriginales = ref([]);
      const texto = ref('');
      const verArticulos = ref(false);
      const verDet = ref(false);
      const valida = ref(false);
      const validaMensaje = ref([]);
      const adModal = ref(false);
      const adAccion = ref(false);
      const adNombre = ref('');
      const adId = ref('');
      const detalles = ref([]);
      const errorArticulo = ref(null);

      const snackbar = ref({
        show: false,
        text: ''
      });
      const totalParcial = computed(() => {
        return detalles.value.reduce((acc, item) => acc + item.precio * item.cantidad, 0);
      });
  
      const totalImpuesto = computed(() => {
        return totalParcial.value * (impuesto.value / 100);
      });
  
      const total = computed(() => {
        return totalParcial.value + totalImpuesto.value;
      });
  
      const actualizarTotal = () => {
        totalParcial.value = detalles.value.reduce((acc, item) => acc + item.precio * item.cantidad, 0);
        totalImpuesto.value = totalParcial.value * (impuesto.value / 100);
        total.value = totalParcial.value + totalImpuesto.value;
     };
  
  
      const mostrarNuevo = () => {
        verNuevo.value = 1;
        verDet.value = 0;

        selectArticulo();
      };
  
      const ocultarNuevo = () => {
        verNuevo.value = 0;
        limpiar();
      };
  
      const buscarCodigo = () => {
        errorArticulo.value = null;
        const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
        const configuracion = { headers: header };
        const codigoTrimmed = codigo.value.trim();
        axios
          .get('api/Articulos/BuscarCodigoIngreso/' + codigoTrimmed + '/', configuracion)
          .then((response) => {
            agregarDetalle(response.data);
          })
          .catch((error) => {
            console.log(error);
            errorArticulo.value = 'No existe el Código artículo';
          });
      };
  
      /*const selectArticulo = () => {
        if (texto.value.trim() === '') {
          articulos.value = [...articulosOriginales.value]; 
          return;
        }
        // Filtrar artículos por el texto ingresado
        articulos.value = articulosOriginales.value.filter(codigo =>
          codigo.codigo.toLowerCase().includes(texto.value.toLowerCase())
        );
      };*/
  
      /*const selectArticulo = () => {
              let me = this;
              var articulosArray=[];
              axios.get('api/Articulos/Listar').then(function(response){
                  articulosArray = response.data;
                  articulosArray.map(function(x){
                      me.almacenes.push({title: x.codigo, value: x.idarticulo})
                  });             
              }).catch(function (error){
                  console.log(error);
              });
  
          };*/
  
    const selectArticulo = async () => {
    try {
      const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
      const response = await axios.get('api/Articulos/Listar', { headers: header });
  
      temporalArticulos.value = response.data.map(y => ({
        agregar: null,
        idarticulo: y.idarticulo,
        codigo: y.codigo || '',
        idcategoria: y.idcategoria,
        descripcion: y.descripcion || '',
        stock: y.stock,
        precio_venta: y.precio_venta || 0
      }));
      articulosOriginales.value = [...temporalArticulos.value];
      filtrarArticulos();
    } catch (error) {
      console.log(error);
    }
  };
  
  const filtrarArticulos = () => {
    if (texto.value.trim() === '') {
      temporalArticulos.value = [...articulosOriginales.value];
    } else {
      const textoBusqueda = texto.value.toLowerCase();
      temporalArticulos.value = articulosOriginales.value.filter(articulo => {
        const descripcionBuscar = articulo.descripcion.toLowerCase().includes(textoBusqueda);
        const codigoBuscar = articulo.codigo.toLowerCase().includes(textoBusqueda);
        const precioVentaBuscar = articulo.precio_venta.toString().includes(textoBusqueda);
        return descripcionBuscar || codigoBuscar || precioVentaBuscar;
      });
    }
  };
  
  
    const mostrarArticulos = () => {
      verArticulos.value = 1;
    };
  
    const ocultarArticulos = () => {
      verArticulos.value = 0;
    };
  
    const agregarDetalle = (data = {}) => {
      errorArticulo.value = null;
  
      if (encuentra(data.idarticulo)) {
        errorArticulo.value = "El artículo ya ha sido agregado.";
        snackbar.value.show = true;
        snackbar.value.text = errorArticulo.value;
      } else {
          detalles.value.push({
          idventa: data.idventa,
          iddetalle_venta: data.idventa,
          idarticulo: data.idarticulo,
          codigo: data.codigo,
          descripcion: data.descripcion,
          cantidad: 1,
          precio: data.precio_venta,
          descuento: data.descuento
  
        });
        actualizarTotal();
      }
    };
  
    const encuentra = (idarticulo) => {
      return detalles.value.some(detalle => detalle.idarticulo === idarticulo);
    };
  

    const borraDetalleVenta = (iddetalle_venta) => {
          const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
          const configuracion = { headers: header };
          axios.delete('api/Ventas/eliminarDetalleVenta/' + iddetalle_venta, configuracion)
          .then((response) => {
              console.log(response);
              //detalles.value = response.data;
              //listarDetalles();
              actualizarTotal();
          }).catch((error) => {
            console.log(error);
          });
        };

        //BORRAR el DETALLE_INGRESO BBS
        const eliminarDetalle = (arr, item) => {
          //console.log("elemento a buscar  ",item);
          console.log("Arreglo a buscar  ", arr);          
          const i = arr.findIndex(arr=>arr.iddetalle_venta===item);
          
          if (i !== -1) {
            arr.splice(i, 1);
            borraDetalleVenta(item);
            actualizarTotal();
            adModal.value=false;
          }
        };
  
    const listar = () => {
      const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
      const configuracion = { headers: header };
      let url = '';
      if (!search.value) {
        url = 'api/Ventas/Listar';
      } else {
        url = 'api/Ventas/ListarFiltro/texto' + search.value;
      }
      //alert("Listar Ventas  " + url);
      axios.get(url, configuracion).then((response) => {
        ventas.value = response.data;
      }).catch((error) => {
        console.log(error);
      });
    };
  
    const listarDetalles = (id) => {
      const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
      const configuracion = { headers: header };
      axios.get('api/Ventas/ListarDetalles/' + id, configuracion).then((response) => {
        detalles.value = response.data;
        actualizarTotal();
      }).catch((error) => {
        console.log(error);
      });
    };
  
    const verDetalles = (item) => {
      limpiar();
      console.log("Ver detalles de:", item);
      idventa.value = item.idventa;
      tipo_comprobante.value = item.tipo_comprobante;
      tipo_movimiento.value = item.tipo_movimiento;
      serie_comprobante.value = item.serie_comprobante;
      num_comprobante.value = item.num_comprobante;
      idarticulo.value = item.idarticulo;
      impuesto.value = item.impuesto;
      listarDetalles(item.idventa);
      verNuevo.value = 1;
      verDet.value = 1;
  
    };
  
  
    const limpiar = () => {
      id.value = "";
      idarticulo.value = "";
      tipo_comprobante.value = "";
      tipo_movimiento.value = "";
      serie_comprobante.value = "";
      num_comprobante.value = "";
      impuesto.value = 16;
      codigo.value = "";
      detalles.value = [];
      total.value = 0;
      totalImpuesto.value = 0;
      totalParcial.value = 0;
      verDet.value = 0;
    };
  
    const actualizar = () => {
  
      if (validar()) {
          return;
      }
  
      const actualizarIngreso = {   
                      idventa: idventa.value,    
                      idcliente: 1009,   
                      idusuario: 1002,
                      tipo_comprobante: tipo_comprobante.value,
                      tipo_movimiento: tipo_movimiento.value,
                      serie_comprobante: serie_comprobante.value,
                      num_comprobante: num_comprobante.value,
                      impuesto:impuesto.value,
                      total:total.value,
                      detalles: detalles.value
       };
     
     const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
     const configuracion = { headers: header };
     axios.put('api/Ventas/Actualizar', actualizarIngreso, configuracion
     ).then((response) => {
              ocultarNuevo(); // Llamar directamente
              listar();
              console.log(response);
          })
          .catch((error) => {
              console.log(error);
          });
    };
  
  
    const guardar = () => {
      // Validar campos antes de guardar
      if (validar()) {
          return;
      }
      // Crear nueva venta
    
      const nuevaVenta = {
          //idventa: ventas.value.length + 1,
          idcliente: 1009,
          idusuario: 1002,
          tipo_comprobante: tipo_comprobante.value,
          tipo_movimiento: tipo_movimiento.value,
          serie_comprobante: serie_comprobante.value,
          num_comprobante: num_comprobante.value,
          //fecha_hora: new Date().toISOString().split('T')[0],
          impuesto: impuesto.value,
          total: total.value,
          estado: 'ACEPTADO',
          detalles: detalles.value
      };
      
      //console.log(nuevaVenta.tipo_movimiento);  // Verifica el valor exacto
  
      // Guardar en la base de datos
      //const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
      //const configuracion = { headers: header };
      //console.log(nuevaVenta);
      axios.post('api/Ventas/Crear', nuevaVenta
     ).then((response) => {
              ocultarNuevo(); // Llamar directamente
              limpiar(); // Llamar directamente
              listar();
              console.log(response);
          })
          .catch((error) => {
              console.log(error);
          });
  };
  
  
  
  
  
    const validar = () => {
      valida.value = 0;
      validaMensaje.value = [];
  
      /*if (!proveedor.value) {
        validaMensaje.value.push("Seleccione un proveedor.");
      }*/
      if (!tipo_comprobante.value) {
        validaMensaje.value.push("Seleccione un tipo de comprobante.");
      }
      if (!tipo_movimiento.value) {
        validaMensaje.value.push("Seleccione un tipo de movimiento.");
      }
      if (!num_comprobante.value) {
        validaMensaje.value.push("Ingrese el número del comprobante.");
      }
      if (!serie_comprobante.value) {
        validaMensaje.value.push("Ingrese el número de serie del comprobante.");
      }
      if (!impuesto.value || impuesto.value < 0) {
        validaMensaje.value.push("Ingrese un impuesto válido.");
      }
      if (validaMensaje.value.length) {
        valida.value = 1;
      }
      return valida.value;
    };
  
    const preguntarBorrarDetalle = (accion, item) => {
    //console.log(accion.index);
    adModal.value = true;
    adNombre.value = item.iddetalle_venta;
    adId.value = item.idventa;
    adAccion.value = accion;
  };
  
  const activarDesactivarCerrar = () => {
    adModal.value = 0;
  };
  
  const desactivar = () => {
    const header = { Authorization: 'Bearer ' + localStorage.getItem('token') };
    const configuracion = { headers: header };
    axios.put('api/Ventas/Anular/' + adId.value, {}, configuracion).then(() => {
      adModal.value = 0;
      adAccion.value = 0;
      adNombre.value = "";
      adId.value = "";
      listar();
    }).catch((error) => {
      console.log(error);
    });
  };
  
  
    /*const muestraDialogo = () => {
      dialogNew.value = true;
    };
  
    const rowClicked = (row) => {
      toggleSelection(row.id);
      console.log(row.id);
    };
  
    const toggleSelection = (keyID) => {
      if (selectedRows.value.includes(keyID)) {
        selectedRows.value = selectedRows.value.filter(
          selectedKeyID => selectedKeyID !== keyID
        );
      } else {
        selectedRows.value.push(keyID);
      }
    };
  
    const initialize = () => {
      // Implementar la lógica de inicialización 
    };
  
    const resetSearch = () => {
      search.value = '';
    };
  
    const resetItems = () => {
      listar();
    };
  
    watch(dialogDelete, (val) => {
      if (!val) closeDelete();
    });
  
    watch(dialogNew, (val) => {
      if (!val) closeNewDialog();
    });*/ 
    
    watch(dialog, (val) => {
      if (!val) close();
    });
  
    onMounted(() => {
      listar();
      selectArticulo();
      guardar();
      actualizar();
     
    });
  
      return {
        ventas,
        headers,
        cabeceraDetalles,
        detalles,
        search,
        idarticulo,
        tipo_comprobante,
        tipo_movimiento,
        comprobantes,
        tipo_movimientos,
        serie_comprobante,
        num_comprobante,
        detalle,
        idventa,
        impuesto,
        codigo,
        verNuevo,
        errorArticulo,
        totalParcial,
        totalImpuesto,
        total,
        cabeceraArticulos,
        articulos,
        texto,
        verArticulos,
        verDet,
        valida,
        validaMensaje,
        adModal,
        adAccion,
        adNombre,
        adId,
        actualizarTotal,
        mostrarNuevo,
        ocultarNuevo,
        selectArticulo,
        mostrarArticulos,
        ocultarArticulos,
        agregarDetalle,
        encuentra,
        eliminarDetalle,
        listar,
        listarDetalles,
        verDetalles,
        limpiar,
        guardar,
        actualizar,
        validar,
        preguntarBorrarDetalle,
        activarDesactivarCerrar,
        desactivar,
        snackbar,
        temporalProveedores,
        temporalArticulos,
        buscarCodigo,
        //idarticulo: ref(null),
        //idusuario: ref(null),
      };
    }
  };
  
  </script>
  
  <style>
    .rojo {
      background: "red";
    }
  
    .tr-blue {
      background-color: aliceblue;
    }
  
    .v-toolbar-title {
      margin-top: 8px;
    }
  
    .v-title-field {
      margin-top: 8px;
    }
    /* eslint-disable*/
  </style>
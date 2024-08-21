using Sistema.Entidades.OrdenTrabajo;
using Sistema.Web.Models.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Orden_de_Trabajo
{
    public class Encabezado_OT_VM
    {
       public int id_ot { get; set; }
       public int id_aeronave { get; set; }
       public string matricula { get; set; }
       public int id_modelo { get; set; }
       public string modelo { get; set; }
        public int id_cliente { get; set; }
       /*public DateTime f_creacion { get; set; }
       public DateTime f_cierre { get; set; }
       public DateTime f_entrega{ get; set; }*/
       public string contacto { get; set; }
       public string telefono { get; set; }
       public string correo { get; set; }
       public string direccion { get; set; }
       public string ciudad { get; set; }
       public string estado { get; set; }
       public string pla_tiempo_total { get; set; }
       public string pla_ciclos_totales { get; set; }
       public string m1_modelo { get; set; }
       public string m1_numero_serie { get; set; }
       public string m1_tiempo_total { get; set; }
       public string m1_ciclos_totales { get; set; }
       public string m2_modelo { get; set; }
       public string m2_numero_serie { get; set; }
       public string m2_tiempo_total { get; set; }
       public string m2_ciclos_totales { get; set; }
       public string apu_modelo { get; set; }
       public string apu_numero_serie { get; set; }
       public string apu_tiempo_total { get; set; }
       public string apu_ciclos_totales { get; set; }
       public List<ProgramadoViewModel> detalles { get; set; }
    }
}

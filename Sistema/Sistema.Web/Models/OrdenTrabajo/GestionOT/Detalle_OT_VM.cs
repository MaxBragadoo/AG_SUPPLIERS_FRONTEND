using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.OrdenTrabajo
{
    public class Detalle_OT_VM
    {
		public int id_detalle { get; set; }
		public int id_ot { get; set; }
		public int id_programada { get; set; }
		public string codigo { get; set; }
		public string descripcion { get; set; }
		public string accion { get; set; }
		public bool programado { get; set; }
		public string intervalo { get; set; }
		public string reporte_discrepancia { get; set; }
		public string no_parte { get; set; }
		public string no_serie { get; set; }
		public string aplica_r2 { get; set; }
	}
}

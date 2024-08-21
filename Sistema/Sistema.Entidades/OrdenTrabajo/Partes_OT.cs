using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entidades.OrdenTrabajo
{
    public class Partes_OT
    {
		public int id_partes { get; set; }
		public int id_ot { get; set; }
		public int id_programada { get; set; }
		public int no_solicitud { get; set; }
		public string codigo { get; set; }
		public int cant_solicitada { get; set; }
		public int cant_sale { get; set; }
		public int cant_requerida { get; set; }
		public decimal precio_venta { get; set; }
		public decimal costo { get; set; }
		public DateTime fecha_real { get; set; }
		public string hora_real { get; set; }
    }
}

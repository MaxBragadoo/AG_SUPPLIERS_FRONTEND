using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.OrdenTrabajo
{
    public class ProgramadoViewModel
    {
        public int id_detalle { get; set; }
        public int id_programada { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string accion { get; set; }
    }
}

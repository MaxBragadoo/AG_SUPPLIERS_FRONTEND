using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Almacen.UnidadMedida
{
    public class UnidadMedidaViewModel
    {
        public int idum { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public bool estatus { get; set; }
        public DateTime f_alta { get; set; }
        public DateTime f_baja { get; set; }
    }
}

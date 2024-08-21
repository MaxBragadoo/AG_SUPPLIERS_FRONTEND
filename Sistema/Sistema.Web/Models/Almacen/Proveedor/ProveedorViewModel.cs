using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Almacen.Proveedor
{
    public class ProveedorViewModel
    {
        public int idproveedor { get; set; }
        public string nombre { get; set; }
        public string nombre_contacto { get; set; }
        public string direccion { get; set; }
        public string codigo_postal { get; set; }
        public string ciudad { get; set; }
        public string telefono1 { get; set; }
        public string telefono2 { get; set; }
        public string correo { get; set; }
        public string notas { get; set; }
        public bool condicion { get; set; }

    }
}

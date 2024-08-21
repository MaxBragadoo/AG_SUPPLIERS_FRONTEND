using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Almacen.Almacenes
{
    public class AlmacenesViewModel
    {
        public int idalmacen { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public bool condicion { get; set; }

    }
}

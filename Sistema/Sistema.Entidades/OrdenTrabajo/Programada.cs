using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entidades.OrdenTrabajo
{
    public class Programada
    {
        public int id_programada { get; set; }
        public string codigo { get; set; }
        public int id_modelo {get; set; }
        public string descripcion { get; set; }
        public string accion { get; set; }
        public Modelo modelo { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Entidades.OrdenTrabajo
{
    public class Select_Modelos_VM
    {
        public int id_modelo { get; set; }
        public string modelo { get; set; }
        public string descripcion { get; set; }
    }
}

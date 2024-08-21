using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Entidades.OrdenTrabajo
{
    public class Select_Aeronaves_VM
    {
        public int id_aeronave { get; set; }
        public string matricula { get; set; }
        public string modelo { get; set; }
    }
}

using Sistema.Entidades.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.OrdenTrabajo
{
    public class Modelo_VM
    {
        public int id_modelo { get; set; }
        public string modelo { get; set; }
        public string descripcion { get; set; }
        public string piston_jet { get; set; }
        public string fabricante { get; set; }
        public string motor_1 { get; set; }
        public string motor_2 { get; set; }
        public string helice_1 { get; set; }
        public string helice_2 { get; set; }
        public string apu { get; set; }

        //public ICollection<Aeronave> aeronaves { get; set; }

    }
}

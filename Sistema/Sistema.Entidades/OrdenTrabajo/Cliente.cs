using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Entidades.OrdenTrabajo
{
    public class Cliente
    {
        public int id_cliente { get; set; }
        public string nombre { get; set; }
        public string nombre_completo { get; set; }
        public string rfc { get; set; }
        public string atencion_a { get; set; }
        public string calle_y_numero { get; set; }
        public string colonia { get; set; }
        public string ciudad { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }
        public string cp { get; set; }
        public string bloqueado { get; set; }
        public string telefono_1 { get; set; }
        public string telefono_2 { get; set; }
        public string email { get; set; }
        public string curp { get; set; }
    }
}

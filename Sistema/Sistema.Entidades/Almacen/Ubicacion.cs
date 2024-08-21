using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema.Entidades.Almacen
{
    public class Ubicacion
    {
        [Required]
        public int idubicacion { get; set; }
        [Required]
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool condicion { get; set; }

        public ICollection<Articulo> articulos { get; set; }


    }
}

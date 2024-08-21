using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema.Entidades.Almacen
{
    public class UnidadMedida
    {
        [Required]
        public int idum { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "El codigo no debe de tener más de 5 caracteres, ni menos de 3 caracteres.")]
        public string codigo { get; set; }

        public string descripcion { get; set; }

        public bool estatus { get; set; }

        //public DateTime f_alta { get; set; }

        //public DateTime f_baja { get; set; }

        public ICollection<Articulo> articulos { get; set; }
        
    }
}

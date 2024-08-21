using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema.Entidades.Almacen
{
    public class Almacenes
    {

        public int idalmacen { get; set; }
        [Required]
        [Range(1, 5000, ErrorMessage = "Can only be between 0 .. 15")]
        public int codigo { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 50 caracteres, ni menos de 3 caracteres.")]
        public string nombre { get; set; }
        public string ubicacion { get; set; }
        public bool condicion { get; set; }
        public ICollection<Articulo> articulos { get; set; }

    }
}

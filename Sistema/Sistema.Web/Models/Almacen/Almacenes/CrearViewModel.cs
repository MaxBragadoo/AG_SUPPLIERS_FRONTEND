using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Web.Models.Almacen.Almacenes
{
    public class CrearViewModel
    {         
        [Required]
        [Range(1, 5000, ErrorMessage = "Can only be between 0 .. 15")]
        public int codigo { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 30 caracteres, ni menos de 3 caracteres.")]
        public string nombre { get; set; }
        public string ubicacion { get; set; }

    }
}

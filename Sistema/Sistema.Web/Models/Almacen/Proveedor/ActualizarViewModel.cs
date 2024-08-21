using System.ComponentModel.DataAnnotations;

namespace Sistema.Web.Models.Almacen.Proveedor
{
    public class ActualizarViewModel
    {
        [Required]
        public int idproveedor { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 50 caracteres, ni menos de 3 caracteres.")]
        public string nombre { get; set; }
        public string nombre_contacto { get; set; }
        public string direccion { get; set; }
        public string codigo_postal { get; set; }
        public string ciudad { get; set; }
        public string telefono1 { get; set; }
        public string telefono2 { get; set; }
        public string correo { get; set; }
        public string notas { get; set; }

    }
}

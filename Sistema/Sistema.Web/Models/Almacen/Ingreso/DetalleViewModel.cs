using System.ComponentModel.DataAnnotations;

namespace Sistema.Web.Models.Almacen.Ingreso
{
    public class DetalleViewModel
    {
        public int iddetalle_ingreso { get; set; }
        [Required]
        public int idingreso { get; set; }
        [Required]
        public int idarticulo { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        [Required]
        public int cantidad { get; set; }
        [Required]
        public decimal precio { get; set; }
    }
}

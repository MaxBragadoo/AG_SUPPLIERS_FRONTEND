using System;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Web.Models.Almacen.Articulo
{
    public class ActualizarViewModel
    {
        [Required]
        public int idarticulo { get; set; }
        [Required]
        public int idcategoria { get; set; }
        [Required]
        public int idalmacen { get; set; }
        [Required]
        public int idum { get; set; }
        [Required]
        public int idproveedor { get; set; }
        [Required]
        public int idubicacion { get; set; }
        [Required]
        public string codigo { get; set; }
        public int stock { get; set; }
        public string descripcion { get; set; }
        //public int cantidad { get; set; }
        //[Required]
        public string numero_serie { get; set; }
        public decimal precio_venta { get; set; }
        public string unidad_medida { get; set; }
        public string localizacion { get; set; }
        public decimal maximo { get; set; }
        public decimal minimo { get; set; }
        public DateTime ultima_entrada { get; set; }
        public DateTime ultima_salida { get; set; }

        public string observaciones { get; set; }
    }
}

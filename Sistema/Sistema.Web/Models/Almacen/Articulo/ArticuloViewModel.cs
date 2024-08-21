using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Models.Almacen.Articulo
{
    public class ArticuloViewModel
    {
        [Required]
        public int idarticulo { get; set; }
        [Required]
        public int idcategoria { get; set; }
        [Required]
        public int idum { get; set; }
        [Required]
        public int idalmacen { get; set; }
        [Required]
        public int idproveedor { get; set; }
        [Required]
        public int idubicacion { get; set; }
        public string almacen { get; set; }
        public string categoria { get; set; }
        public string unidadMedida { get; set; }
        public string proveedor { get; set; }
        public string ubicacion { get; set; }
        [Required]
        public string codigo { get; set; }
        [Required]
        public string numero_serie { get; set; }
        public decimal precio_venta { get; set; }
        public int stock { get; set; }
        public string descripcion { get; set; }
        public bool condicion { get; set; }
        public string localizacion { get; set; }
        public decimal maximo { get; set; }
        public decimal minimo { get; set; }
        public DateTime ultima_entrada { get; set; }
        public DateTime ultima_salida { get; set; }    
    }
}

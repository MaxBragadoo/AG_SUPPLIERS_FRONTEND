using System.ComponentModel.DataAnnotations;

namespace Sistema.Web.Models.Almacen.Articulo
{
    public class CrearViewModel
    {
       // [Required]
        public int idcategoria { get; set; }
        public int idalmacen { get; set; }
        public int idum { get; set; }
        public int idproveedor { get; set; }
        public int idubicacion { get; set; }
        public string codigo { get; set; }
        public int stock { get; set; }
        public string descripcion { get; set; }
        //public int cantidad { get; set; }
        public string numero_serie { get; set; }        
        public decimal precio_venta { get; set; }    
        public string unidad_medida { get; set; }     
        public string localizacion { get; set; }
        public decimal maximo { get; set; }
        public decimal minimo { get; set; }
        public string observaciones { get; set; }
    }
}

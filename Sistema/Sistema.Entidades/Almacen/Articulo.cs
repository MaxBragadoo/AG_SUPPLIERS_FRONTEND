using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Entidades.Almacen
{
    public class Articulo
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
        [StringLength(50, MinimumLength = 3,ErrorMessage = "El nombre no debe de tener más de 50 caracteres, ni menos de 3 caracteres.")]
        public string numero_serie { get; set; }
        public string descripcion { get; set; }
        public string localizacion { get; set; }
        public bool condicion { get; set; }
        [Required]
        public decimal precio_venta { get; set; }
        public int stock { get; set; }
        public decimal maximo { get; set; }
        public decimal minimo { get; set; }

        //[DataType(DataType.PhoneNumber)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ultima_entrada { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]        
        public DateTime ultima_salida { get; set; }
        public string observaciones { get; set; }
        public Categoria categoria { get; set; }        
        public Almacenes almacen { get; set; }
        public UnidadMedida unidadMedida { get; set; }
        public Proveedor proveedor { get; set; }
        public Ubicacion ubicacion { get; set; }
        public ICollection<DetalleIngreso> DetallesIngresos { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema.Entidades.OrdenTrabajo
{
    public class Modelo
    {
       public int id_modelo { get; set; }
        [StringLength(8, MinimumLength = 3,
        ErrorMessage = "El nombre no debe de tener más de 50 caracteres, ni menos de 3 caracteres.")]
       public string modelo { get; set; }
       public string descripcion { get; set; }
       public string piston_jet { get; set; }
       public string fabricante { get;  set; }
       public string motor_1 { get; set;  }
       public string motor_2 { get; set; }
       public string helice_1 { get; set; }
       public string helice_2 { get; set; }
       public string apu { get; set; }
       public ICollection<Aeronave> aeronaves { get; set;  }

    }
}

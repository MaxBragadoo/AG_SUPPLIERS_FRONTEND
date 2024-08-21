using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Web.Models.Orden_de_Trabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.OrdenTrabajo
{
    class EncabezadoOtMap : IEntityTypeConfiguration<Encabezado_OT>
    {
        public void Configure(EntityTypeBuilder<Encabezado_OT> builder)
        {
            builder.ToTable("ot_encabezado")
               .HasKey(o => o.id_ot);
        }
    }
}

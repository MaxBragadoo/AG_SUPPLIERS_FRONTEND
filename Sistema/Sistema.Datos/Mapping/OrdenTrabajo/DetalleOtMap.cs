using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.OrdenTrabajo
{
    public class DetalleOtMap : IEntityTypeConfiguration<Detalle_OT>
    {
        public void Configure(EntityTypeBuilder<Detalle_OT> builder)
        {
            builder.ToTable("ot_detalle")
                 .HasKey(d => d.id_detalle);
        }
    }
}

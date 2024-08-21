using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.OrdenTrabajo
{
    public class ModeloAeronaveMap : IEntityTypeConfiguration<Modelo>
    {
        public void Configure(EntityTypeBuilder<Modelo> builder)
        {
            builder.ToTable("ot_modelo")
                .HasKey(a => a.id_modelo);
        }
    }
}

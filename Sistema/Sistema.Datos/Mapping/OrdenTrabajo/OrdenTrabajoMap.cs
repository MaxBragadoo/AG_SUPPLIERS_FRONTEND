using Microsoft.EntityFrameworkCore;
using Sistema.Entidades.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.OrdenTrabajo
{
    public class OrdenTrabajoMap : IEntityTypeConfiguration<Programado>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Programado> builder)
        {
            builder.ToTable("ot_programado")
            .HasKey(p => p.id_programado);
        }
    }
}

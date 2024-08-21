using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.OrdenTrabajo
{
    public class ProgramadaMap : IEntityTypeConfiguration<Programada>
    {
        public void Configure(EntityTypeBuilder<Programada> builder)
        {
            builder.ToTable("ot_programada")
                .HasKey(p => p.id_programada);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Almacen
{
    public class UbicacionMap : IEntityTypeConfiguration<Ubicacion>
    {
        public void Configure(EntityTypeBuilder<Ubicacion> builder)
        {
            builder.ToTable("ubicacion")
               .HasKey(u => u.idubicacion);
            builder.Property(u => u.nombre)
                .HasMaxLength(30);
            builder.Property(u => u.descripcion)
                .HasMaxLength(256);
        }
    }
}

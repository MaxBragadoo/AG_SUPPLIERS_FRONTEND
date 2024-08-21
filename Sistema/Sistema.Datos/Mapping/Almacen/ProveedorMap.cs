using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.Almacen
{
    public class ProveedorMap : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("proveedor")
               .HasKey(p => p.idproveedor);
            builder.Property(p => p.nombre)
                .HasMaxLength(50);
            builder.Property(p => p.notas)
                .HasMaxLength(256);
        }
    }
}

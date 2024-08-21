using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sistema.Datos.Mapping.Almacen
{
    public class AlmacenesMap : IEntityTypeConfiguration<Almacenes>
    {
        public void Configure(EntityTypeBuilder<Almacenes> builder)
        {
            builder.ToTable("almacenes")
               .HasKey(a => a.idalmacen);
                

        }
    }
}

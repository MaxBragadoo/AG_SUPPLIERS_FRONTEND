using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema.Entidades.OrdenTrabajo;
using Sistema.Web.Models.Orden_de_Trabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos.Mapping.OrdenTrabajo
{
    class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("ot_cliente")
               .HasKey(o => o.id_cliente);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Sistema.Datos.Mapping.Almacen;
using Sistema.Datos.Mapping.OrdenTrabajo;
using Sistema.Datos.Mapping.Usuarios;
using Sistema.Datos.Mapping.Ventas;
using Sistema.Entidades.Almacen;
using Sistema.Entidades.OrdenTrabajo;
using Sistema.Entidades.Usuarios;
using Sistema.Entidades.Ventas;
using Sistema.Web.Models.Orden_de_Trabajo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema.Datos
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }      
        public DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<DetalleIngreso> DetallesIngresos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVentas { get; set; }
        public DbSet<Almacenes> Almacenes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }

        public DbSet<Programada> Programadas { get; set; }
        public DbSet<Encabezado_OT> Encabezados_OT { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Detalle_OT> Detalles_OT { get; set; }
       // public DbSet<Partes_OT> Partes_OT { get; set; }
        public DbSet<Modelo> Modelos { get; set; }
        public DbSet<Aeronave> Aeronaves { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new UnidadMedidaMap());
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());
            modelBuilder.ApplyConfiguration(new AlmacenesMap());
            modelBuilder.ApplyConfiguration(new ProveedorMap());
            modelBuilder.ApplyConfiguration(new UbicacionMap());

            modelBuilder.ApplyConfiguration(new ModeloAeronaveMap());
            modelBuilder.ApplyConfiguration(new AeronaveMap());
            modelBuilder.ApplyConfiguration(new ProgramadaMap());
            modelBuilder.ApplyConfiguration(new EncabezadoOtMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new DetalleOtMap());
        }

    }
}

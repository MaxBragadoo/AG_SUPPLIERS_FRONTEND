using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Ventas;
using Sistema.Web.Models.Ventas.Venta;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public VentasController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Ventas/Listar
        //[Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> Listar()
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .OrderByDescending(v => v.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento=v.persona.num_documento,
                direccion=v.persona.direccion,
                telefono=v.persona.telefono,
                email=v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                tipo_movimiento = v.tipo_movimiento, 
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                //fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });

        }

        // GET: api/Ventas/VentasMes12
       /* [Authorize(Roles = "Almacenero,Vendedor,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ConsultaViewModel>> VentasMes12()
        {
            var consulta = await _context.Ventas
                //GroupBy(v=>v.fecha_hora.Month)
                .Select(x=> new { etiqueta=x.Key, valor =x.Sum(v=>v.total)})
                .OrderByDescending(x =>x.etiqueta)
                .Take(12)
                .ToListAsync();

            return consulta.Select(v => new ConsultaViewModel
            {
                etiqueta=v.etiqueta.ToString(),
                valor=v.valor
            });

        }*/

        // GET: api/Ventas/ListarFiltro/texto
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<VentaViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                .Where(v => v.num_comprobante.Contains(texto))
                .OrderByDescending(v => v.idventa)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                telefono = v.persona.telefono,
                email = v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                //fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });

        }

        // GET: api/Ventas/ConsultaFechas/FechaInicio/FechaFin
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{FechaInicio}/{FechaFin}")]
        public async Task<IEnumerable<VentaViewModel>> ConsultaFechas([FromRoute] DateTime FechaInicio,DateTime FechaFin)
        {
            var venta = await _context.Ventas
                .Include(v => v.usuario)
                .Include(v => v.persona)
                //.Where(i=>i.fecha_hora>=FechaInicio)
                //.Where(i => i.fecha_hora <= FechaFin)
                .OrderByDescending(v => v.idventa)
                .Take(100)
                .ToListAsync();

            return venta.Select(v => new VentaViewModel
            {
                idventa = v.idventa,
                idcliente = v.idcliente,
                cliente = v.persona.nombre,
                num_documento = v.persona.num_documento,
                direccion = v.persona.direccion,
                telefono = v.persona.telefono,
                email = v.persona.email,
                idusuario = v.idusuario,
                usuario = v.usuario.nombre,
                tipo_comprobante = v.tipo_comprobante,
                serie_comprobante = v.serie_comprobante,
                num_comprobante = v.num_comprobante,
                //fecha_hora = v.fecha_hora,
                impuesto = v.impuesto,
                total = v.total,
                estado = v.estado
            });

        }

        // GET: api/Ventas/ListarDetalles
        //[Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{idventa}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idventa)
        {
            var detalle = await _context.DetallesVentas
                .Include(a => a.articulo)
                .Where(d => d.idventa == idventa)
                .ToListAsync();

            return detalle.Select(d => new DetalleViewModel
            {
                idventa = d.idventa,
                iddetalle_venta = d.iddetalle_venta,
                idarticulo = d.idarticulo,
                codigo = d.articulo.codigo,
                descripcion = d.articulo.descripcion,
                cantidad = d.cantidad,
                precio = d.precio,
                descuento=d.descuento
            });

        }

        // POST: api/Ventas/Crear
       // [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fechaHora = DateTime.Now;

            Venta venta = new Venta
            {
                idcliente = model.idcliente,
                idusuario = model.idusuario,
                tipo_comprobante = model.tipo_comprobante,
                tipo_movimiento = model.tipo_movimiento,
                serie_comprobante = model.serie_comprobante,
                num_comprobante = model.num_comprobante,
                //fecha_hora = fechaHora,
                impuesto = model.impuesto,
                total = model.total,
                estado = "Aceptado"
            };


            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var id = venta.idventa;
                foreach (var det in model.detalles)
                {
                    DetalleVenta detalle = new DetalleVenta
                    {
                        idventa = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio,
                        descuento=det.descuento
                    };
                    _context.DetallesVentas.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Ventas/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idventa <= 0)
            {
                return BadRequest();
            }

            var ventas = await _context.Ventas.FirstOrDefaultAsync(c => c.idventa == model.idventa);

            if (ventas == null)
            {
                return NotFound();
            }

            ventas.idusuario = model.idusuario;
            ventas.tipo_comprobante = model.tipo_comprobante;
            ventas.tipo_movimiento = model.tipo_movimiento;
            ventas.serie_comprobante = model.serie_comprobante;
            ventas.num_comprobante = model.num_comprobante;
            ventas.impuesto = model.impuesto;
            ventas.total = model.total;

            try
            {
                await _context.SaveChangesAsync();
                var id = ventas.idventa;

                foreach (var det in model.detalles)
                {
                    if (det.iddetalle_venta <= 0)
                    {
                        DetalleVenta detalle = new DetalleVenta
                        {
                            idventa = id,
                            idarticulo = det.idarticulo,
                            cantidad = det.cantidad,
                            precio = det.precio,
                            descuento = det.descuento
                        };
                        _context.DetallesVentas.Add(detalle);
                    }
                }
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Ventas/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> eliminarDetalleVenta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var detalle_venta = await _context.DetallesVentas.FindAsync(id);
            if (detalle_venta == null)
            {
                return NotFound();
            }

            _context.DetallesVentas.Remove(detalle_venta);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(detalle_venta);
        }

        // PUT: api/Ventas/Anular/1
        [Authorize(Roles = "Vendedor,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas.FirstOrDefaultAsync(v => v.idventa == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
                // Inicio de código para devolver stock
                // 1. Obtenemos los detalles
                var detalle = await _context.DetallesVentas.Include(a => a.articulo).Where(d => d.idventa == id).ToListAsync();
                //2. Recorremos los detalles
                foreach (var det in detalle)
                {
                    //Obtenemos el artículo del detalle actual
                    var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.idarticulo == det.articulo.idarticulo);
                    //actualizamos el stock
                    articulo.stock = det.articulo.stock + det.cantidad;
                    //Guardamos los cambios
                    await _context.SaveChangesAsync();
                }
                // Fin del código para devolver stock
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }


        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.idventa == id);
        }
    }
}
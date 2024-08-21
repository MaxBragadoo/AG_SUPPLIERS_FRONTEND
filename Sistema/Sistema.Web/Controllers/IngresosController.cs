using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Almacen.Ingreso;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public IngresosController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Ingresos/Listar
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<IngresoViewModel>> Listar()
        {
            var ingreso = await _context.Ingresos
                .Include(i => i.usuario)
                .Include(i=>i.proveedor)
                .OrderByDescending(i=>i.idingreso)
                .Take(100)
                .ToListAsync();

            return ingreso.Select(i => new IngresoViewModel
            {
                idingreso           = i.idingreso,
                idproveedor         = i.idproveedor,
                proveedor           = i.proveedor.nombre,
                idusuario           = i.idusuario,
                usuario             = i.usuario.nombre,
                tipo_comprobante    = i.tipo_comprobante,
                tipo_movimiento     = i.tipo_movimiento,
                serie_comprobante   = i.serie_comprobante,
                num_comprobante     = i.num_comprobante,
                //fecha_hora          = i.fecha_hora,
                impuesto            = i.impuesto,
                total               = i.total,
                estado              = i.estado
            });
        }

        // GET: api/Ingresos/ListarFiltro/texto
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<IngresoViewModel>> ListarFiltro([FromRoute] string texto)
        {
            var ingreso = await _context.Ingresos
                .Include(i => i.usuario)
                .Include(i => i.proveedor)
                .Where(i=>i.num_comprobante.Contains(texto))
                .OrderByDescending(i => i.idingreso)
                .ToListAsync();

            return ingreso.Select(i => new IngresoViewModel
            {
                idingreso           = i.idingreso,
                idproveedor         = i.idproveedor,
                proveedor           = i.proveedor.nombre,
                idusuario           = i.idusuario,
                usuario             = i.usuario.nombre,
                tipo_comprobante    = i.tipo_comprobante,
                tipo_movimiento     = i.tipo_movimiento,
                serie_comprobante   = i.serie_comprobante,
                num_comprobante     = i.num_comprobante,
                //fecha_hora          = i.fecha_hora,
                impuesto            = i.impuesto,
                total               = i.total,
                estado              = i.estado
            });
        }

        // GET: api/Ingresos/ListarDetalles
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{idingreso}")]
        public async Task<IEnumerable<DetalleViewModel>> ListarDetalles([FromRoute] int idingreso)
        {
            var detalle = await _context.DetallesIngresos
                .Include(a => a.articulo)
                .Where(d=>d.idingreso==idingreso)
                .ToListAsync();

            return detalle.Select(d => new DetalleViewModel
            {
                iddetalle_ingreso   = d.iddetalle_ingreso,
                idarticulo          = d.idarticulo,
                codigo              = d.articulo.codigo,
                descripcion         = d.articulo.descripcion,
                cantidad            = d.cantidad,
                precio              = d.precio
            });
        }
        
        // POST: api/Ingresos/Crear
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var fechaHora = DateTime.Now;

            Ingreso ingreso = new Ingreso {
                idproveedor         = model.idproveedor,
                idusuario           = model.idusuario,
                tipo_comprobante    = model.tipo_comprobante,
                tipo_movimiento     = model.tipo_movimiento,
                serie_comprobante   = model.serie_comprobante,
                num_comprobante     = model.num_comprobante,
                fecha_hora          = fechaHora,
                impuesto            = model.impuesto,
                total               = model.total,
                estado              = "Aceptado"
            };     
            
            try
            {
                _context.Ingresos.Add(ingreso);
                await _context.SaveChangesAsync();

                var id = ingreso.idingreso;
                foreach (var det in model.detalles)
                {
                    DetalleIngreso detalle = new DetalleIngreso
                    {
                        idingreso = id,
                        idarticulo = det.idarticulo,
                        cantidad = det.cantidad,
                        precio = det.precio
                    };
                    _context.DetallesIngresos.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Ingresos/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idingreso <= 0)
            {
                return BadRequest();
            }

            var ingresos = await _context.Ingresos.FirstOrDefaultAsync(c => c.idingreso == model.idingreso);

            if (ingresos == null)
            {
                return NotFound();
            }
      
            ingresos.idproveedor        = model.idproveedor;
            ingresos.idusuario          = model.idusuario;
            ingresos.tipo_comprobante   = model.tipo_comprobante;
            ingresos.tipo_movimiento    = model.tipo_movimiento;
            ingresos.serie_comprobante  = model.serie_comprobante;
            ingresos.num_comprobante    = model.num_comprobante;
            ingresos.impuesto           = model.impuesto;
            ingresos.total              = model.total;

            try
            {
                await _context.SaveChangesAsync();
                var id = ingresos.idingreso;

                foreach (var det in model.detalles)
                {
                    if (det.iddetalle_ingreso <= 0)
                    {
                        DetalleIngreso detalle = new DetalleIngreso
                        {
                            idingreso = id,
                            idarticulo = det.idarticulo,
                            cantidad = det.cantidad,
                            precio = det.precio
                        };
                        _context.DetallesIngresos.Add(detalle);
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

        // PUT: api/Ingresos/Anular/1
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Anular([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var ingreso = await _context.Ingresos.FirstOrDefaultAsync(c=>c.idingreso == id);

            if (ingreso == null)
            {
                return NotFound();
            }

            ingreso.estado = "Anulado";

            try
            {
                await _context.SaveChangesAsync();
                
                // Inicio de código para devolver stock
                // 1. Obtenemos los detalles
                var detalle = await _context.DetallesIngresos.Include(a => a.articulo).Where(d => d.idingreso == id).ToListAsync();
                //2. Recorremos los detalles
                foreach (var det in detalle)
                {
                    //Obtenemos el artículo del detalle actual
                    var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.idarticulo == det.articulo.idarticulo);
                    //actualizamos el stock
                    articulo.stock = det.articulo.stock-det.cantidad;
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

        // DELETE: api/ingresos/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> eliminarDetalleIngreso([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var detalle_ingreso = await _context.DetallesIngresos.FindAsync(id);
            if (detalle_ingreso == null)
            {
                return NotFound();
            }

            _context.DetallesIngresos.Remove(detalle_ingreso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(detalle_ingreso);
        }


        /* List<DetalleIngreso> dept = new List<DetalleIngreso>()
               {
                   new DetalleIngreso() { iddetalle_ingreso=4031 },
                   new DetalleIngreso() { iddetalle_ingreso=4032 },
                   new DetalleIngreso() { iddetalle_ingreso=4033 }
               };

               _context.RemoveRange(dept);
               await _context.SaveChangesAsync();*/
        private bool IngresoExists(int id)
        {
            return _context.Ingresos.Any(e => e.idingreso == id);
        }
    }
}
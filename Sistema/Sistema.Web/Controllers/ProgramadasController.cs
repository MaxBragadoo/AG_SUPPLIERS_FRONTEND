
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.OrdenTrabajo;
using Sistema.Web.Models.OrdenTrabajo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramadaController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ProgramadaController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Programada/Listar
        // [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<Programada_VM>> Listar()
        {
            var programada = await _context.Programadas.Include(m=>m.modelo).ToListAsync();

            return programada.Select(a => new Programada_VM
            {
                id_programada   = a.id_programada,
                codigo          = a.codigo,
                id_modelo       = a.id_modelo,
                modelo          = a.modelo.modelo,
                descripcion     = a.descripcion,
                accion          = a.accion,
            });
        }

        // GET: api/Programada/Listar_x_modelo/1
        // [Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{modelo_ot}")]
        public async Task<IEnumerable<Programada_VM>> Listar_x_modelo([FromRoute] string modelo_ot)
        {
            var modelo_ot_temp = modelo_ot;


            var programada = await _context.Programadas.Include(m => m.modelo).Where(p=>p.modelo.modelo==modelo_ot).ToListAsync();

            return programada.Select(a => new Programada_VM
            {
                id_programada = a.id_programada,
                codigo = a.codigo,
                id_modelo = a.id_modelo,
                modelo = a.modelo.modelo,
                descripcion = a.descripcion,
                accion = a.accion,
            });
        }


        // PUT: api/Programada/Actualizar
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] Programada_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id_programada <= 0)
            {
                return BadRequest();
            }

            var programada = await _context.Programadas.FirstOrDefaultAsync(a => a.id_programada == model.id_programada);

            if (programada == null)
            {
                return NotFound();
            }

            programada.id_programada = model.id_programada;
            programada.codigo = model.codigo;
            programada.id_modelo = model.id_modelo;
            programada.descripcion = model.descripcion;
            programada.accion = model.accion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Articulos/Crear
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] Programada_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Programada vm_programada = new Programada
            {
                id_modelo   = model.id_modelo,
                codigo      = model.codigo,
                descripcion = model.descripcion,
                accion      = model.accion,
            };

            _context.Programadas.Add(vm_programada);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }


        // GET: api/Articulos/ListarIngreso/texto
        /*          [Authorize(Roles = "Almacenero,Administrador")]
             [HttpGet("[action]/{texto}")]
             public async Task<IEnumerable<ArticuloViewModel>> ListarIngreso([FromRoute] string texto)
             {
                 var articulo = await _context.Articulos.Include(a => a.categoria)
                     .Where(a => a.nombre.Contains(texto))
                     .Where(a => a.condicion == true)
                     .ToListAsync();

                 return articulo.Select(a => new ArticuloViewModel
                 {
                     idarticulo = a.idarticulo,
                     idcategoria = a.idcategoria,
                     categoria = a.categoria.nombre,
                     codigo = a.codigo,
                     nombre = a.nombre,
                     stock = a.stock,
                     precio_venta = a.precio_venta,
                     descripcion = a.descripcion,
                     condicion = a.condicion
                 });

             }

             // GET: api/Articulos/ListarVenta/texto
             [Authorize(Roles = "Vendedor,Administrador")]
             [HttpGet("[action]/{texto}")]
             public async Task<IEnumerable<ArticuloViewModel>> ListarVenta([FromRoute] string texto)
             {
                 var articulo = await _context.Articulos.Include(a => a.categoria)
                     .Where(a => a.nombre.Contains(texto))
                     .Where(a => a.condicion == true)
                     .Where(a => a.stock > 0)
                     .ToListAsync();

                 return articulo.Select(a => new ArticuloViewModel
                 {
                     idarticulo = a.idarticulo,
                     idcategoria = a.idcategoria,
                     categoria = a.categoria.nombre,
                     codigo = a.codigo,
                     nombre = a.nombre,
                     stock = a.stock,
                     precio_venta = a.precio_venta,
                     descripcion = a.descripcion,
                     condicion = a.condicion
                 });

             }


             // GET: api/Articulos/Mostrar/1
             [Authorize(Roles = "Almacenero,Administrador")]
             [HttpGet("[action]/{id}")]
             public async Task<IActionResult> Mostrar([FromRoute] int id)
             {

                 var articulo = await _context.Articulos.Include(a => a.categoria).
                     SingleOrDefaultAsync(a => a.idarticulo == id);

                 if (articulo == null)
                 {
                     return NotFound();
                 }

                 return Ok(new ArticuloViewModel
                 {
                     idarticulo = articulo.idarticulo,
                     idcategoria = articulo.idcategoria,
                     categoria = articulo.categoria.nombre,
                     codigo = articulo.codigo,
                     nombre = articulo.nombre,
                     descripcion = articulo.descripcion,
                     stock = articulo.stock,
                     precio_venta = articulo.precio_venta,
                     condicion = articulo.condicion
                 });
             }

             // GET: api/Articulos/BuscarCodigoIngreso/12345678
             [Authorize(Roles = "Almacenero,Administrador")]
             [HttpGet("[action]/{codigo}")]
             public async Task<IActionResult> BuscarCodigoIngreso([FromRoute] string codigo)
             {

                 var articulo = await _context.Articulos.Include(a => a.categoria)
                     .Where(a => a.condicion == true).
                     SingleOrDefaultAsync(a => a.codigo == codigo);

                 if (articulo == null)
                 {
                     return NotFound();
                 }

                 return Ok(new ArticuloViewModel
                 {
                     idarticulo = articulo.idarticulo,
                     idcategoria = articulo.idcategoria,
                     categoria = articulo.categoria.nombre,
                     codigo = articulo.codigo,
                     nombre = articulo.nombre,
                     descripcion = articulo.descripcion,
                     stock = articulo.stock,
                     precio_venta = articulo.precio_venta,
                     condicion = articulo.condicion
                 });
             }


             // GET: api/Articulos/BuscarCodigoVenta/12345678
             [Authorize(Roles = "Vendedor,Administrador")]
             [HttpGet("[action]/{codigo}")]
             public async Task<IActionResult> BuscarCodigoVenta([FromRoute] string codigo)
             {

                 var articulo = await _context.Articulos.Include(a => a.categoria)
                     .Where(a => a.condicion == true)
                     .Where(a => a.stock > 0)
                     .SingleOrDefaultAsync(a => a.codigo == codigo);

                 if (articulo == null)
                 {
                     return NotFound();
                 }

                 return Ok(new ArticuloViewModel
                 {
                     idarticulo = articulo.idarticulo,
                     idcategoria = articulo.idcategoria,
                     categoria = articulo.categoria.nombre,
                     codigo = articulo.codigo,
                     nombre = articulo.nombre,
                     descripcion = articulo.descripcion,
                     stock = articulo.stock,
                     precio_venta = articulo.precio_venta,
                     condicion = articulo.condicion
                 });
             }


             
             // PUT: api/Articulos/Desactivar/1
             [Authorize(Roles = "Almacenero,Administrador")]
             [HttpPut("[action]/{id}")]
             public async Task<IActionResult> Desactivar([FromRoute] int id)
             {

                 if (id <= 0)
                 {
                     return BadRequest();
                 }

                 var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.idarticulo == id);

                 if (articulo == null)
                 {
                     return NotFound();
                 }

                 articulo.condicion = false;

                 try
                 {
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     // Guardar Excepción
                     return BadRequest();
                 }

                 return Ok();
             }

             // PUT: api/Articulos/Activar/1
             [Authorize(Roles = "Almacenero,Administrador")]
             [HttpPut("[action]/{id}")]
             public async Task<IActionResult> Activar([FromRoute] int id)
             {

                 if (id <= 0)
                 {
                     return BadRequest();
                 }

                 var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.idarticulo == id);

                 if (articulo == null)
                 {
                     return NotFound();
                 }

                 articulo.condicion = true;

                 try
                 {
                     await _context.SaveChangesAsync();
                 }
                 catch (DbUpdateConcurrencyException)
                 {
                     // Guardar Excepción
                     return BadRequest();
                 }

                 return Ok();
             }


             private bool ArticuloExists(int id)
             {
                 return _context.Articulos.Any(e => e.idarticulo == id);
             }
         }*/
    }
}

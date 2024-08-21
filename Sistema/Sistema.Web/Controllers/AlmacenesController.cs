using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Almacen.Almacenes;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacenesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public AlmacenesController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Almacenes/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<AlmacenesViewModel>> Listar()
        {
            var almacen = await _context.Almacenes.ToListAsync();

            return almacen.Select(a => new AlmacenesViewModel
            {
                idalmacen = a.idalmacen,
                codigo = a.codigo,
                nombre = a.nombre,
                ubicacion = a.ubicacion,
                condicion = a.condicion
            });

        }

        // GET: api/Almacenes/Select
        //Metodo que nos sirve para poder llenar el select de Almacenes en la vista Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectPrvViewModel>> Select()
        {
            var almacen = await _context.Almacenes.Where(c => c.condicion == true).ToListAsync();

            return almacen.Select(a => new SelectPrvViewModel
            {
                idalmacen = a.idalmacen,
                nombre = a.nombre
            });

        }

        // GET: api/Almacenes/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {

            var almacen = await _context.Almacenes.FindAsync(id);

            if (almacen == null)
            {
                return NotFound();
            }

            return Ok(new AlmacenesViewModel
            {
                idalmacen = almacen.idalmacen,
                codigo = almacen.codigo,
                nombre = almacen.nombre,
                ubicacion = almacen.ubicacion,
                condicion = almacen.condicion
            });
        }

        // PUT: api/Almacenes/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idalmacen <= 0)
            {
                return BadRequest();
            }

            var almacen = await _context.Almacenes.FirstOrDefaultAsync(a => a.idalmacen == model.idalmacen);

            if (almacen == null)
            {
                return NotFound();
            }

            almacen.codigo = model.codigo;
            almacen.nombre = model.nombre;
            almacen.ubicacion = model.ubicacion;

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

        //Metodo para poder crear un nuevo almacen
        // POST: api/Almacenes/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Almacenes almacen = new Almacenes
            {
                codigo = model.codigo,
                nombre = model.nombre,
                ubicacion = model.ubicacion,
                condicion = true
            };

            _context.Almacenes.Add(almacen);
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

        // DELETE: api/Almacenes/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var almacen = await _context.Almacenes.FindAsync(id);
            if (almacen == null)
            {
                return NotFound();
            }

            _context.Almacenes.Remove(almacen);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(almacen);
        }

        // PUT: api/Almacenes/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var almacen = await _context.Almacenes.FirstOrDefaultAsync(a => a.idalmacen == id);

            if (almacen == null)
            {
                return NotFound();
            }

            almacen.condicion = false;

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

        // PUT: api/Almacenes/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var almacen = await _context.Almacenes.FirstOrDefaultAsync(a => a.idalmacen == id);

            if (almacen == null)
            {
                return NotFound();
            }

            almacen.condicion = true;

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

        private bool CategoriaExists(int id)
        {
            return _context.Almacenes.Any(a => a.idalmacen == id);
        }


    }
}

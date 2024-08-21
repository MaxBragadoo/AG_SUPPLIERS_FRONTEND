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
using Sistema.Web.Models.Almacen.Ubicacion;

namespace Sistema.Web.Controllers
{
    //[Authorize(Roles = "Almacenero,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionesController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public UbicacionesController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Ubicaciones/Listar
        [HttpGet("[action]")]
        public async Task <IEnumerable<UbicacionViewModel>> Listar()
        {
            var ubicacion = await _context.Ubicaciones.ToListAsync();

            return ubicacion.Select(c => new UbicacionViewModel
            {
                idubicacion = c.idubicacion,
                nombre = c.nombre,
                descripcion = c.descripcion,
                condicion = c.condicion
            });

        }

        // GET: api/Ubicaciones/Select
        //Metodo que nos sirve para poder llenar el select de Categorias en la vista Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectViewModel>> Select()
        {
            var ubicacion = await _context.Ubicaciones.Where(u=>u.condicion==true).ToListAsync();

            return ubicacion.Select(u => new SelectViewModel
            {
                idubicacion = u.idubicacion,
                nombre = u.nombre
            });

        }

        // GET: api/Ubicaciones/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {

            var ubicacion = await _context.Ubicaciones.FindAsync(id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            return Ok(new UbicacionViewModel {
                idubicacion = ubicacion.idubicacion,
                nombre = ubicacion.nombre,
                descripcion = ubicacion.descripcion,
                condicion = ubicacion.condicion
            });
        }

        // PUT: api/Ubicaciones/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idubicacion <= 0)
            {
                return BadRequest();
            }

            var ubicacion = await _context.Ubicaciones.FirstOrDefaultAsync(c => c.idubicacion == model.idubicacion);

            if (ubicacion == null)
            {
                return NotFound();
            }

            ubicacion.nombre = model.nombre;
            ubicacion.descripcion = model.descripcion;

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

        // POST: api/Ubicaciones/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Ubicacion ubicacion = new Ubicacion
            {
                nombre = model.nombre,
                descripcion = model.descripcion,
                condicion = true
            };

            _context.Ubicaciones.Add(ubicacion);
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

        // DELETE: api/Ubicaciones/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ubicacion = await _context.Ubicaciones.FindAsync(id);
            if (ubicacion == null)
            {
                return NotFound();
            }

            _context.Ubicaciones.Remove(ubicacion);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }           

            return Ok(ubicacion);
        }

        // PUT: api/Ubicaciones/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var ubicacion = await _context.Ubicaciones.FirstOrDefaultAsync(u => u.idubicacion == id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            ubicacion.condicion = false;

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

        // PUT: api/Ubicaciones/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var ubicacion = await _context.Ubicaciones.FirstOrDefaultAsync(u => u.idubicacion == id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            ubicacion.condicion = true;

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

        private bool UbicacionExists(int id)
        {
            return _context.Ubicaciones.Any(u => u.idubicacion == id);
        }
    }
}
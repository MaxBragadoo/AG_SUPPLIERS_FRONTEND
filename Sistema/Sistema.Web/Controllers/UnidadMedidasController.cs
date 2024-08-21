using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Almacen;
using Sistema.Web.Models.Almacen.UnidadMedida;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadMedidasController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public UnidadMedidasController(DbContextSistema context)
        {
            _context = context;
        }

        //Metodo que nos sireve para poder listar las unidades de medida.
        // GET: api/UnidadMedidas/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<UnidadMedidaViewModel>> Listar()
        {
            var unidadMedida = await _context.UnidadMedidas.ToListAsync();

            return unidadMedida.Select(u => new UnidadMedidaViewModel
            {
                idum = u.idum,
                codigo = u.codigo,
                descripcion = u.descripcion,
                estatus = u.estatus,
                // f_alta = u.f_alta,
                //f_baja = u.f_baja                
            });

        }

        // GET: api/UnidadMedidas/Select
        //Metodo que nos sirve para poder llenar el select de Unidad de Medida en la vista Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectUMViewModel>> Select()
        {
            var unidadMedida = await _context.UnidadMedidas.Where(u => u.estatus == true).ToListAsync();

            return unidadMedida.Select(u => new SelectUMViewModel
            {
                idum = u.idum,
                codigo = u.codigo,
                descripcion = u.descripcion,
            });

        }

        //Metodo para poder Mostrar una Unidad de Medida por su ID
        // GET: api/UnidadMedidas/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {

            var unidadMedida = await _context.UnidadMedidas.FindAsync(id);

            if (unidadMedida == null)
            {
                return NotFound();
            }

            return Ok(new UnidadMedidaViewModel
            {
                idum        = unidadMedida.idum,
                codigo      = unidadMedida.codigo,
                descripcion = unidadMedida.descripcion,
                estatus     = unidadMedida.estatus,
                //f_alta      = unidadMedida.f_alta,
               //f_baja      = unidadMedida.f_baja
            });
        }
        //Metodo para poder Actualizar una Unidad de Medida
        // PUT: api/UnidadMedidas/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idum <= 0)
            {
                return BadRequest();
            }

            var unidadMedida = await _context.UnidadMedidas.FirstOrDefaultAsync(u => u.idum == model.idum);

            if (unidadMedida == null)
            {
                return NotFound();
            }

            unidadMedida.codigo      = model.codigo;
            unidadMedida.descripcion = model.descripcion;
            //unidadMedida.f_alta      = model.f_alta;
           // unidadMedida.f_baja = model.f_baja;

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
        //Metodo para poder crear una nueva Unidad de medida
        // POST: api/UnidadMedidas/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UnidadMedida unidadMedida = new UnidadMedida
            {
                codigo = model.codigo,
                descripcion = model.descripcion,
                estatus = true,
                //f_alta = model.f_alta,
               // f_baja = model.f_baja
            };

            _context.UnidadMedidas.Add(unidadMedida);
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

        // DELETE: api/UnidadMedidas/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unidadMedida = await _context.UnidadMedidas.FindAsync(id);
            if (unidadMedida == null)
            {
                return NotFound();
            }

            _context.UnidadMedidas.Remove(unidadMedida);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok(unidadMedida);
        }

        // PUT: api/UnidadMedidas/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var unidadMedida = await _context.UnidadMedidas.FirstOrDefaultAsync(u => u.idum == id);

            if (unidadMedida == null)
            {
                return NotFound();
            }

            unidadMedida.estatus = false;

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

        // PUT: api/UnidadMedidas/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var unidadMedida = await _context.UnidadMedidas.FirstOrDefaultAsync(u => u.idum == id);

            if (unidadMedida == null)
            {
                return NotFound();
            }

            unidadMedida.estatus = true;

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




        private bool UnidadMedidaExists(int id)
        {
            return _context.UnidadMedidas.Any(u => u.idum == id);
        }
    }
}


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
using Sistema.Web.Models.Almacen.Proveedor;

namespace Sistema.Web.Controllers
{
    //[Authorize(Roles = "Almacenero,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ProveedoresController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Proveedores/Listar
        [HttpGet("[action]")]
        public async Task <IEnumerable<ProveedorViewModel>> Listar()
        {
            var proveedor = await _context.Proveedores.ToListAsync();

            return proveedor.Select(p => new ProveedorViewModel
            {
                idproveedor = p.idproveedor,
                nombre = p.nombre,
                nombre_contacto = p.nombre_contacto,
                direccion = p.direccion,
                codigo_postal = p.codigo_postal,
                ciudad = p.ciudad,
                telefono1 = p.telefono1,
                telefono2 = p.telefono2,
                correo = p.correo,
                notas = p.notas,
                condicion = p.condicion
            });

        }
        
        // GET: api/Categorias/Select
        //Metodo que nos sirve para poder llenar el select de Categorias en la vista Articulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<SelectPrvViewModel>> Select()
        {
            var proveedor = await _context.Proveedores.Where(p=>p.condicion==true).ToListAsync();

            return proveedor.Select(c => new SelectPrvViewModel
            {
                idproveedor = c.idproveedor,
                nombre = c.nombre
            });

        } 

        // GET: api/Proveedores/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {

            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return Ok(new ProveedorViewModel {
                idproveedor = proveedor.idproveedor,
                nombre = proveedor.nombre,
                nombre_contacto = proveedor.nombre_contacto,
                direccion = proveedor.direccion,
                codigo_postal = proveedor.codigo_postal,
                ciudad = proveedor.ciudad,
                telefono1 = proveedor.telefono1,
                telefono2 = proveedor.telefono2,
                correo = proveedor.correo,
                notas = proveedor.notas
            });
        }

        // PUT: api/Proveedores/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idproveedor <= 0)
            {
                return BadRequest();
            }

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.idproveedor == model.idproveedor);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.nombre            = model.nombre;
            proveedor.nombre_contacto   = model.nombre_contacto;
            proveedor.direccion         = model.direccion;
            proveedor.codigo_postal     = model.codigo_postal;
            proveedor.ciudad            = model.ciudad;
            proveedor.telefono1         = model.telefono1;
            proveedor.telefono2         = model.telefono2;
            proveedor.correo            = model.correo;
            proveedor.notas             = model.notas;

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

        // POST: api/Proveedores/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Proveedor proveedor = new Proveedor
            {
                nombre          = model.nombre,
                nombre_contacto = model.nombre_contacto,
                direccion       = model.direccion,
                codigo_postal   = model.codigo_postal,
                ciudad          = model.ciudad,
                telefono1       = model.telefono1,
                telefono2       = model.telefono2,
                correo          = model.correo,
                notas           = model.notas,
                condicion       = true
            };

            _context.Proveedores.Add(proveedor);
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

        // DELETE: api/Proveedores/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            _context.Proveedores.Remove(proveedor);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }           

            return Ok(proveedor);
        }

        // PUT: api/Proveedores/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.idproveedor == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.condicion = false;

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

        // PUT: api/Proveedores/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.idproveedor == id);

            if (proveedor == null)
            {
                return NotFound();
            }

            proveedor.condicion = true;

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

        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(p => p.idproveedor == id);
        }
    }

}
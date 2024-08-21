using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Almacen;
using Sistema.Entidades.OrdenTrabajo;
using Sistema.Web.Models.Almacen.Categoria;
using Sistema.Web.Models.Orden_de_Trabajo;
using Sistema.Web.Models.OrdenTrabajo;

namespace Sistema.Web.Controllers
{
    //[Authorize(Roles = "Almacenero,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ClienteController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/OrdenTrabajo/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<Cliente_VM>> Listar()
        {
            var Cliente = await _context.Clientes.ToListAsync();

            return Cliente.Select(c => new Cliente_VM
            {

                id_cliente          = c.id_cliente,
                nombre              = c.nombre, 
                nombre_completo     = c.nombre_completo,
                rfc                 = c.rfc,
                atencion_a          = c.atencion_a,
                calle_y_numero      = c.calle_y_numero,
                colonia             = c.colonia,
                ciudad              = c.ciudad,
                estado              = c.estado,
                pais                = c.pais,
                cp                  = c.cp,
                telefono_1          = c.telefono_1,
                telefono_2          = c.telefono_2,
                email               = c.email,
                curp                = c.curp,
            });

        }

        // PUT: api/OrdenTrabajo/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] Cliente_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id_cliente <= 0)
            {
                return BadRequest();
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.id_cliente == model.id_cliente);

            if (cliente == null)
            {
                return NotFound();
            }

            //cliente.id_cliente      = model.id_cliente;
            cliente.nombre          = model.nombre;
            cliente.rfc             = model.rfc;
            cliente.atencion_a      = model.atencion_a;
            cliente.calle_y_numero  = model.calle_y_numero;
            cliente.colonia         = model.colonia;
            cliente.ciudad          = model.ciudad;
            cliente.estado          = model.estado;
            cliente.pais            = model.pais;
            cliente.cp              = model.cp;
            cliente.telefono_1      = model.telefono_1;
            cliente.telefono_2      = model.telefono_2;
            cliente.email           = model.email;
            cliente.curp            = model.curp;

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

        // POST: api/Categorias/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] Cliente_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Cliente cliente = new Cliente            
            {               
                nombre          = model.nombre,
                rfc             = model.rfc,
                atencion_a      = model.atencion_a,    
                calle_y_numero  = model.calle_y_numero,
                colonia         = model.colonia,
                ciudad          = model.ciudad,
                estado          = model.estado,
                pais            = model.pais,
                cp              = model.cp,
                telefono_1      = model.telefono_1,
                telefono_2      = model.telefono_2,
                email           = model.email,
                curp            = model.curp,
            };

            _context.Clientes.Add(cliente);
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
        [HttpGet("[action]")]
        public async Task<IEnumerable<Cliente_VM>> Select()
        {
            var cliente = await _context.Clientes.ToListAsync();

            return cliente.Select(c => new Cliente_VM
            {
                id_cliente  = c.id_cliente,
                nombre      = c.nombre
            });

        }

        // GET: api/Categorias/Listar
        /*  // GET: api/Categorias/Mostrar/1
                   [HttpGet("[action]/{id}")]
              public async Task<IActionResult> Mostrar([FromRoute] int id)
              {

                  var categoria = await _context.Categorias.FindAsync(id);

                  if (categoria == null)
                  {
                      return NotFound();
                  }

                  return Ok(new CategoriaViewModel
                  {
                      idcategoria = categoria.idcategoria,
                      nombre = categoria.nombre,
                      descripcion = categoria.descripcion,
                      condicion = categoria.condicion
                  });
              }


             

              // DELETE: api/Categorias/Eliminar/1
              [HttpDelete("[action]/{id}")]
              public async Task<IActionResult> Eliminar([FromRoute] int id)
              {
                  if (!ModelState.IsValid)
                  {
                      return BadRequest(ModelState);
                  }

                  var categoria = await _context.Categorias.FindAsync(id);
                  if (categoria == null)
                  {
                      return NotFound();
                  }

                  _context.Categorias.Remove(categoria);
                  try
                  {
                      await _context.SaveChangesAsync();
                  }
                  catch (Exception ex)
                  {
                      return BadRequest();
                  }

                  return Ok(categoria);
              }

              // PUT: api/Categorias/Desactivar/1
              [HttpPut("[action]/{id}")]
              public async Task<IActionResult> Desactivar([FromRoute] int id)
              {

                  if (id <= 0)
                  {
                      return BadRequest();
                  }

                  var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.idcategoria == id);

                  if (categoria == null)
                  {
                      return NotFound();
                  }

                  categoria.condicion = false;

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

              // PUT: api/Categorias/Activar/1
              [HttpPut("[action]/{id}")]
              public async Task<IActionResult> Activar([FromRoute] int id)
              {

                  if (id <= 0)
                  {
                      return BadRequest();
                  }

                  var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.idcategoria == id);

                  if (categoria == null)
                  {
                      return NotFound();
                  }

                  categoria.condicion = true;

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
                  return _context.Categorias.Any(e => e.idcategoria == id);
              }*/
    }
}
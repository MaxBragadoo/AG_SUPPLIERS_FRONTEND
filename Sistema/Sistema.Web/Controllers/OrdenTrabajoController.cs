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
    public class OrdenTrabajoController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public OrdenTrabajoController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/OrdenTrabajo/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<Encabezado_OT_VM>> Listar()
        {
            var OrdenTrabajo = await _context.Encabezados_OT
                .Include(a => a.aeronave)
                .Include(a => a.aeronave.modelo)
                .ToListAsync();

            return OrdenTrabajo.Select(c => new Encabezado_OT_VM
            {
                id_ot       = c.id_ot,
                id_aeronave = c.id_aeronave, 
                matricula   = c.aeronave.matricula,
                id_modelo   = c.aeronave.id_modelo,
                modelo      = c.aeronave.modelo.modelo,
                id_cliente  = c.id_cliente,
                /*f_cierre    = c.f_cierre,
                f_creacion  = c.f_creacion,
                f_entrega   = c.f_entrega,*/
                contacto    = c.contacto,
                telefono    = c.telefono,
                correo      = c.correo,
                direccion   = c.direccion,
                ciudad      = c.direccion,
                estado      = c.estado,
            });

        }

        // PUT: api/OrdenTrabajo/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] Encabezado_OT_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id_ot <= 0)
            {
                return BadRequest();
            }

            var encabezado_ot = await _context.Encabezados_OT.FirstOrDefaultAsync(c => c.id_ot == model.id_ot);

            if (encabezado_ot == null)
            {
                return NotFound();
            }

            encabezado_ot.id_aeronave = model.id_aeronave;
            encabezado_ot.id_cliente  = model.id_cliente;
            encabezado_ot.contacto    = model.contacto; 
            encabezado_ot.telefono    = model.telefono;
            encabezado_ot.correo      = model.correo;
            encabezado_ot.direccion   = model.direccion;
            encabezado_ot.estado      = model.estado;
            encabezado_ot.ciudad      = model.ciudad;            

            try
            {   
                await _context.SaveChangesAsync();
                var id = encabezado_ot.id_ot;

                foreach (var det in  model.detalles)
                {
                    if (det.id_detalle <= 0 ){
                        Detalle_OT detalle = new Detalle_OT
                        {
                            id_ot = id,
                            id_programada = det.id_programada,
                            codigo = det.codigo,
                            descripcion = det.descripcion
                        };
                        _context.Detalles_OT.Add(detalle);
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

        // POST: api/OrdenTrabajo/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] Encabezado_OT_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Encabezado_OT encabezado_ot = new Encabezado_OT
            {
                id_aeronave = model.id_aeronave,
                id_cliente = model.id_cliente,
                contacto = model.contacto,
                telefono = model.telefono,
                correo = model.correo,
                direccion = model.direccion,
                estado = model.estado,
                ciudad = model.ciudad,
            };

            try
            {
                _context.Encabezados_OT.Add(encabezado_ot);
                await _context.SaveChangesAsync();

                var id = encabezado_ot.id_ot;
                foreach (var det in model.detalles)
                {
                    Detalle_OT detalle = new Detalle_OT
                    {
                        id_ot = id,
                        id_programada = det.id_programada,
                        codigo = det.codigo,
                        descripcion = det.descripcion
                    };
                    _context.Detalles_OT.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        // PUT: api/OrdenTrabajo/Actualizar
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar_Programada([FromBody] Detalle_OT_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id_detalle <= 0)
            {
                return BadRequest();
            }

            var programada = await _context.Detalles_OT.FirstOrDefaultAsync(a => a.id_detalle == model.id_detalle);

            if (programada == null)
            {
                return NotFound();
            }

           // programada.id_programada = model.id_programada;
            programada.codigo = model.codigo;
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

        // GET: api/Ordentrabajo/ListarDetalles/1
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{id_ot}")]
        public async Task<IEnumerable<Detalle_OT_VM>> ListarDetalles([FromRoute] int id_ot)
        {
            var detalle = await _context.Detalles_OT
                .Where(d => d.id_ot == id_ot)
                .ToListAsync();

            return detalle.Select(d => new Detalle_OT_VM
            {

                id_detalle      = d.id_detalle,
                id_programada   = d.id_programada,
                codigo          = d.codigo,
                descripcion     = d.descripcion,
                accion          = d.accion
            });

        }


        // GET: api/Categorias/Listar
        /* [HttpGet("[action]")]
              public async Task<IEnumerable<SelectViewModel>> Select()
              {
                  var categoria = await _context.Categorias.Where(c => c.condicion == true).ToListAsync();

                  return categoria.Select(c => new SelectViewModel
                  {
                      idcategoria = c.idcategoria,
                      nombre = c.nombre
                  });

              }

              // GET: api/Categorias/Mostrar/1
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
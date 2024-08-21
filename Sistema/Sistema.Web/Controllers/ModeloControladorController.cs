using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sistema.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Web.Models.OrdenTrabajo;
using Sistema.Entidades.OrdenTrabajo;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeloControladorController : ControllerBase
    {
        private readonly DbContextSistema _context;
        public ModeloControladorController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/modelocontrolador/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<Modelo_VM>> Listar()
        {
            var vmodelo = await _context.Modelos.ToListAsync();

            return vmodelo.Select(c => new Modelo_VM
            {
                id_modelo   = c.id_modelo,
                modelo      = c.modelo,
                descripcion = c.descripcion,
                piston_jet  = c.piston_jet,
                fabricante  = c.fabricante,
                motor_1     = c.motor_1,
                motor_2     = c.motor_2,
                helice_1    = c.helice_1,
                helice_2    = c.helice_2,
                apu         = c.apu,
            });

        }

        //PUT: api/modelocontrolador/Actualizar"
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] Modelo_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id_modelo <= 0)
            {
                return BadRequest();
            }

            var modelo = await _context.Modelos.FirstOrDefaultAsync(m => m.id_modelo == model.id_modelo);

            if (modelo == null)
            {
                return NotFound();
            }

            modelo.modelo = model.modelo;
            modelo.descripcion = model.descripcion;
            modelo.piston_jet = model.piston_jet;
            modelo.motor_1 = model.motor_1;
            modelo.motor_2 = model.motor_2;
            modelo.helice_1 = model.helice_1;
            modelo.helice_2 = model.helice_2;
            modelo.apu = model.apu;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();

            }

            return Ok();
        }

        // POST: api/modelocontrolador/crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] Modelo_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Modelo modelo = new Modelo
            {
                modelo      = model.modelo,
                descripcion = model.descripcion,
                piston_jet  = model.piston_jet,
                motor_1     = model.motor_1,
                motor_2     = model.motor_2,
                helice_1    = model.helice_1,
                helice_2    = model.helice_2,
                apu         = model.apu,
            };

            _context.Modelos.Add(modelo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest();
                }
            return Ok();
        }


        // GET: api/modelocontrolador/select
        [HttpGet("[action]")]
        public async Task<IEnumerable<Select_Modelos_VM>> Select()
        {
            var selectModelos = await _context.Modelos.ToListAsync();

            return selectModelos.Select(c => new Select_Modelos_VM
            {
                id_modelo  = c.id_modelo,
                modelo = c.modelo
            });
        }

        // GET: api/ModelosControlador/Mostrar/1
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {            
            var modelo_vm = await _context.Modelos.SingleOrDefaultAsync(a => a.id_modelo == id);

            if (modelo_vm == null)
            {
                return NotFound();
            }

            return Ok(new Modelo_VM
            {
                id_modelo   = modelo_vm.id_modelo,
                modelo      = modelo_vm.modelo,
                descripcion = modelo_vm.descripcion,
                piston_jet  = modelo_vm.piston_jet,
                fabricante  = modelo_vm.fabricante,
                motor_1     = modelo_vm.motor_1,
                motor_2     = modelo_vm.motor_2,
                helice_1    = modelo_vm.helice_1,
                helice_2    = modelo_vm.helice_2,
                apu         = modelo_vm.apu
            });
        }
    }
}

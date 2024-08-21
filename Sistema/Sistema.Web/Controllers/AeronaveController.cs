using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Sistema.Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sistema.Entidades.OrdenTrabajo;
using Microsoft.EntityFrameworkCore;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AeronaveController : ControllerBase
    {
        private readonly DbContextSistema _context;
        public AeronaveController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Aeronave/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<ActualizarAeronaveViewModel>> Listar()
        {
            var vaeronave = await _context.Aeronaves.Include(m=>m.modelo).ToListAsync();

            return vaeronave.Select(c => new ActualizarAeronaveViewModel
            {
                id_modelo = c.id_modelo,
                id_aeronave = c.id_aeronave,
                matricula = c.matricula,
                modelo = c.modelo.modelo,
                ns_planeador = c.ns_planeador,
                unidad = c.unidad,
                //fabricacion = c.fabricacion,
                capitan = c.capitan,
                tel_capitan = c.tel_capitan,
                cliente = c.cliente,
            }); ;

        } 
        //PUT: api/Aeronave/Actualizar"
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] Actualizar_Aeronave_VM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.id_aeronave <= 0)
            {
                return BadRequest();
            }

            var modelo = await _context.Aeronaves.FirstOrDefaultAsync(m => m.id_aeronave == model.id_aeronave);

            if (modelo == null)
            {
                return NotFound();
            }

            modelo.id_modelo = model.id_modelo;
            modelo.matricula = model.matricula;
            modelo.ns_planeador = model.ns_planeador;
            modelo.unidad = model.unidad;
            modelo.capitan = model.capitan;
            modelo.tel_capitan = model.tel_capitan;
            modelo.cliente = model.cliente;


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

        // POST: api/Aeronave/crear
        [HttpPost("[action]")]
         public async Task<IActionResult> Crear([FromBody] ActualizarAeronaveViewModel model)
         {
             if (!ModelState.IsValid)
             {
                 return BadRequest();
             }
       
            Aeronave modelo = new Aeronave
            {
                 matricula    = model.matricula,
                 id_modelo    = model.id_modelo,
                 ns_planeador = model.ns_planeador,
                 unidad       = model.unidad,
                 capitan      = model.capitan,
                 tel_capitan  = model.tel_capitan
            };

             _context.Aeronaves.Add(modelo);
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


        // GET: api/aeronave/select
        [HttpGet("[action]")]
        public async Task<IEnumerable<Select_Aeronaves_VM>> Select()
        {
            var selectAeronaves = await _context.Aeronaves.Include(m=>m.modelo).ToListAsync();

            return selectAeronaves.Select(c => new Select_Aeronaves_VM
            {
                id_aeronave = c.id_aeronave,
                matricula   = c.matricula,
                modelo      = c.modelo.modelo
            });
        }

        // GET: api/aeronave/regresamodelo/1
        [HttpGet("[action]/{id_modelo}")]
        public async Task<IEnumerable<Select_Aeronaves_VM>> RegresaModelo([FromRoute] int id_modelo)
        {
            var selectAeronaves = await _context.Aeronaves.Include(m => m.modelo).Where(m => m.id_aeronave== id_modelo).ToListAsync();

            return selectAeronaves.Select(c => new Select_Aeronaves_VM
            {
                id_aeronave = c.id_aeronave,
                matricula = c.matricula,
                modelo = c.modelo.modelo
            });
        }
    }
}

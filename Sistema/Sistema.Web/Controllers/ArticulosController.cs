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
using Sistema.Web.Models.Almacen.Articulo;

namespace Sistema.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public ArticulosController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Articulos/Listar
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> Listar()
        {
            var articulo = await _context.Articulos
                .Include(a => a.almacen)
                .Include(a => a.categoria)
                .Include(a => a.unidadMedida)
                .Include(a => a.proveedor)
                .Include(a => a.ubicacion)
                .ToListAsync();

            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo      = a.idarticulo,
                idalmacen       = a.idalmacen,
                idcategoria     = a.idcategoria,
                idum            = a.idum,
                idproveedor     = a.idproveedor,
                idubicacion     = a.idubicacion,
                almacen         = a.almacen.nombre,
                categoria       = a.categoria.nombre,
                unidadMedida    = a.unidadMedida.codigo,
                proveedor       = a.proveedor.nombre,
                codigo          = a.codigo,
                numero_serie    = a.numero_serie,
                stock           = a.stock,
                precio_venta    = a.precio_venta,
                descripcion     = a.descripcion,
                ubicacion       = a.ubicacion.nombre,
                localizacion    = a.localizacion,
                condicion       = a.condicion,
                maximo          = a.maximo,
                minimo          = a.minimo,
                ultima_entrada  = a.ultima_entrada,
                ultima_salida   = a.ultima_salida,
               // prueba_fecha    = a.ultima_entrada.ToString(),

            });

        }

        // GET: api/Articulos/ListarIngreso/texto
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarIngreso([FromRoute] string texto)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria)
                .Include(a => a.unidadMedida)
                .Where(a=>a.codigo.Contains(texto))
                .Where(a=>a.condicion==true)
                .ToListAsync();

            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                idum = a.idum,
                idalmacen = a.idalmacen,
                idproveedor = a.idproveedor,
                idubicacion = a.idubicacion,
                categoria = a.categoria.nombre,
                unidadMedida = a.unidadMedida.codigo,
                proveedor = a.proveedor.nombre,
                ubicacion = a.ubicacion.nombre,
                codigo = a.codigo,
                numero_serie = a.numero_serie,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion,
            });
        }

        // GET: api/Articulos/ListarVenta/texto
        //[Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{texto}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarVenta([FromRoute] string texto)
        {
            var articulo = await _context.Articulos.Include(a => a.categoria)
                .Where(a => a.numero_serie.Contains(texto))
                .Where(a => a.condicion == true)
                .Where(a=>a.stock>0)
                .ToListAsync();

            return articulo.Select(a => new ArticuloViewModel
            {
                idarticulo = a.idarticulo,
                idcategoria = a.idcategoria,
                idubicacion = a.idubicacion,
                categoria = a.categoria.nombre,
                codigo = a.codigo,
                numero_serie = a.numero_serie,
                stock = a.stock,
                precio_venta = a.precio_venta,
                descripcion = a.descripcion,
                condicion = a.condicion,
                //unidad_medida = a.unidad_medida,
                //ubicacion = a.ubicacion
            });

        }


        // GET: api/Articulos/Mostrar/1
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
            var articulo = await _context.Articulos
                .Include(a=>a.almacen)
                .Include(a=>a.categoria)
                .Include(a=>a.unidadMedida)
                .Include(a=>a.proveedor)
                .Include(a=>a.ubicacion)
                .SingleOrDefaultAsync(a=>a.idarticulo==id);

            if (articulo == null)
            {
                return NotFound();
            }

            return Ok(new ArticuloViewModel
            {
                idarticulo  = articulo.idarticulo,
                idalmacen   = articulo.idalmacen,
                idcategoria = articulo.idcategoria,
                idum        = articulo.idum,
                idproveedor = articulo.idproveedor,
                idubicacion = articulo.idubicacion,
                almacen     = articulo.almacen.nombre,
                categoria   = articulo.categoria.nombre,
                ubicacion   = articulo.ubicacion.nombre,
                unidadMedida= articulo.unidadMedida.codigo,
                proveedor   = articulo.proveedor.nombre,               
                codigo      = articulo.codigo,
                numero_serie = articulo.numero_serie,
                descripcion = articulo.descripcion,
                stock       = articulo.stock,
                precio_venta = articulo.precio_venta,
                condicion   = articulo.condicion,
               
                
            });
        }

        // GET: api/Articulos/BuscarCodigoIngreso/12345678
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpGet("[action]/{codigo}")]
        public async Task<IActionResult> BuscarCodigoIngreso([FromRoute] string codigo)
        {

            var articulo = await _context.Articulos.Include(a => a.categoria)
                .Where(a=>a.condicion==true).
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
                numero_serie = articulo.numero_serie,
                descripcion = articulo.descripcion,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                condicion = articulo.condicion,
                //unidad_medida = articulo.unidad_medida,
                //ubicacion = articulo.ubicacion
            });
        }


        // GET: api/Articulos/BuscarCodigoVenta/12345678
        //[Authorize(Roles = "Vendedor,Administrador")]
        [HttpGet("[action]/{codigo}")]
        public async Task<IActionResult> BuscarCodigoVenta([FromRoute] string codigo)
        {

            var articulo = await _context.Articulos.Include(a => a.categoria)
                .Where(a => a.condicion == true)
                .Where(a=>a.stock>0)
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
                numero_serie = articulo.numero_serie,
                descripcion = articulo.descripcion,
                stock = articulo.stock,
                precio_venta = articulo.precio_venta,
                condicion = articulo.condicion,
                //unidad_medida = articulo.unidad_medida,
                //ubicacion = articulo.ubicacion
            });
        }


        // PUT: api/Articulos/Actualizar
        //[Authorize(Roles = "Almacenero,Administrador")]
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idarticulo <= 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.idarticulo == model.idarticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.idcategoria    = model.idcategoria;
            articulo.idalmacen      = model.idalmacen;
            articulo.idcategoria    = model.idcategoria;
            articulo.idum           = model.idum;
            articulo.idproveedor    = model.idproveedor;
            articulo.idubicacion    = model.idubicacion;
            articulo.codigo         = model.codigo;
            articulo.numero_serie   = model.numero_serie;
            articulo.precio_venta   = model.precio_venta;
            articulo.localizacion   = model.localizacion;
            articulo.stock          = model.stock;
            articulo.descripcion    = model.descripcion;
            articulo.maximo         = model.maximo;
            articulo.minimo         = model.minimo;
            articulo.ultima_entrada = model.ultima_entrada;
            articulo.ultima_salida  = model.ultima_salida;


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
        public async Task<IActionResult> Crear([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Articulo articulo = new Articulo
            {
                idcategoria     = model.idcategoria,
                idalmacen       = model.idalmacen,
                idum            = model.idum,
                idproveedor     = model.idproveedor,
                idubicacion     = model.idubicacion,
                codigo          = model.codigo,
                numero_serie    = model.numero_serie,
                precio_venta    = model.precio_venta,
                localizacion    = model.localizacion,
                stock           = model.stock,
                descripcion     = model.descripcion,                
                condicion       = true,
                maximo          = model.maximo,
                minimo          = model.minimo,
        };

            _context.Articulos.Add(articulo);
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

        // PUT: api/Articulos/Desactivar/1
        //[Authorize(Roles = "Almacenero,Administrador")]
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
        //[Authorize(Roles = "Almacenero,Administrador")]
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
    }
}
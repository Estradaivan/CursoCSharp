using GestionVentas.Models;
using GestionVentas.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : Controller
    {
        private VentasRepository repository = new VentasRepository();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Venta> lista = repository.listarVenta();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Venta> Get(long id)
        {
            try
            {
                Venta? venta = repository.obtenerVenta(id);
                if (venta != null)
                {
                    return Ok(venta);
                }
                else
                {
                    return NotFound("La venta no fue encontrada");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Venta venta)
        {
            try
            {
                Venta ventaCreado = repository.crearVenta(venta);
                return StatusCode(StatusCodes.Status201Created, ventaCreado);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool seElimino = repository.eliminarVenta(id);
                if (seElimino)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Producto> Put(long id, [FromBody] Venta ventaAActualizar)
        {
            try
            {
                Venta? ventaActualizado = repository.actualizarVenta(id, ventaAActualizar);
                if (ventaActualizado != null)
                {
                    return Ok(ventaActualizado);
                }
                else
                {
                    return NotFound("La venta no fue encontrada");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

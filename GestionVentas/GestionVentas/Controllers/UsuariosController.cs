using GestionVentas.Models;
using GestionVentas.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private UsuariosRepository repository = new UsuariosRepository();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Usuario> lista = repository.listarUsuario();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> Get(long id)
        {
            try
            {
                Usuario? usuario = repository.obtenerUsuario(id);
                if (usuario != null)
                {
                    return Ok(usuario);
                }
                else
                {
                    return NotFound("El usuario no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Usuario usuario)
        {
            try
            {
                Usuario usuarioCreado = repository.crearUsuario(usuario);
                return StatusCode(StatusCodes.Status201Created, usuarioCreado);
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
                bool seElimino = repository.eliminarUsuario(id);
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
        public ActionResult<Usuario> Put(long id, [FromBody] Usuario usuarioAActualizar)
        {
            try
            {
                Usuario? usuarioActualizado = repository.actualizarUsuario(id, usuarioAActualizar);
                if (usuarioActualizado != null)
                {
                    return Ok(usuarioActualizado);
                }
                else
                {
                    return NotFound("El usuario no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using UsuariosLibrary;
using UsuariosLibrary.Models;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly Methods _usuario;
        public UsuariosController() { 
            _usuario = new Methods();
        }

        [HttpGet]
        [Route("")]
        public dynamic Get()
        {
            try
            {
                var request = _usuario.Get();
                
                if (request == null) { return NotFound(); };

                return request;

            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("login")]
        public dynamic Login([FromBody] Usuario usuario)
        {
            try
            {
                var request = _usuario.Login(usuario);

                if (request == null) { return StatusCode(401, "Email or password are incorrect, please try again"); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("permisos")]
        public dynamic GetPermisos()
        {
            try
            {
                var request = _usuario.GetPermisos();

                if (request == null) { return NotFound(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public dynamic GetById(int id)
        {
            try
            {
                var request = _usuario.GetById(id);

                if (request == null) { return NotFound(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("")]
        public dynamic Create([FromBody] Usuario usuario)
        {
            try
            {
                var request = _usuario.Create(usuario);

                if (request == null) { return BadRequest(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public dynamic Update(int id, [FromBody] Usuario usuario)
        {
            try
            {
                var request = _usuario.Update(id, usuario);

                if (request == null) { return NotFound(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public dynamic Delete(int id)
        {
            try
            {
                var request = _usuario.Delete(id);

                if (request == null) { return NotFound(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

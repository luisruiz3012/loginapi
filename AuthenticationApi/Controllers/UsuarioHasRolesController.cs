using Microsoft.AspNetCore.Mvc;
using UsuarioHasRolesLibrary;
using UsuarioHasRolesLibrary.Models;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/usuario-has-roles")]
    public class UsuarioHasRolesController : ControllerBase
    {
        private readonly Methods uhr;

        public UsuarioHasRolesController()
        {
            uhr = new Methods();
        }

        [HttpGet]
        [Route("")]
        public dynamic Get()
        {
            try
            {
                var request = uhr.Get();

                if (request == null) { return NotFound(); };

                return request;

            } catch (Exception ex)
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
                var request = uhr.GetById(id);

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
        public dynamic Create([FromBody] UsuarioHasRole usuarioRoles)
        {
            try
            {
                var request = uhr.Create(usuarioRoles);

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
        public dynamic Update(int id, [FromBody] UsuarioHasRole usuarioRoles)
        {
            try
            {
                var request = uhr.Update(id, usuarioRoles);

                if (request == null) { return NotFound(); };

                return request;

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("")]
        public dynamic Delete(int id)
        {
            try
            {
                var request = uhr.Delete(id);

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

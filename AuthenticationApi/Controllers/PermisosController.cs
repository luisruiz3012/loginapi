using Microsoft.AspNetCore.Mvc;
using PermisosLibrary;
using PermisosLibrary.Models;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/permisos")]
    public class PermisosController : ControllerBase
    {
        private readonly Methods _permisos;
        public PermisosController() {
            _permisos = new Methods();
        }

        [HttpGet]
        [Route("")]
        public dynamic Get()
        {
            try
            {
                var request = _permisos.Get();

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
                var request = _permisos.GetById(id);

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
        public dynamic Create([FromBody] Permiso permiso)
        {
            try
            {
                var request = _permisos.Create(permiso);

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
        public dynamic Update(int id, [FromBody] Permiso permiso)
        {
            try
            {
                var request = _permisos.Update(id, permiso);

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
                var request = _permisos.Delete(id);

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

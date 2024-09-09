using Microsoft.AspNetCore.Mvc;
using RolesLibrary;
using RolesLibrary.Models;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : ControllerBase
    {
        private readonly Methods _roles;
        public RolesController() { 
            _roles = new Methods();
        }

        [HttpGet]
        [Route("")]
        public dynamic Get()
        {
            try
            {
                var request = _roles.Get();

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
                var request = _roles.GetById(id);

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
        public dynamic Create([FromBody] Rol rol)
        {
            try
            {
                var request = _roles.Create(rol);

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
        public dynamic Update(int id, [FromBody] Rol rol)
        {
            try
            {
                var request = _roles.Update(id, rol);

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
                var request = _roles.Delete(id);

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

using Core.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoSalaoDeBeleza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly ServicesService _serviceService;

        public ServiceController(ServicesService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest("Service is null.");
            }

            var createdService = await _serviceService.CreateServiceAsync(service);
            return CreatedAtAction(nameof(GetServiceById), new { id = createdService.Id }, createdService);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var service = await _serviceService.GetServiceByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _serviceService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] Service service)
        {
            if (service == null || service.Id != id)
            {
                return BadRequest("Service is null or ID mismatch.");
            }

            try
            {
                await _serviceService.UpdateServiceAsync(id, service);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            try
            {
                await _serviceService.DeleteServiceAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

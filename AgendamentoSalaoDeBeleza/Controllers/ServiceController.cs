using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoSalaoDeBeleza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly SalonContext _context;

        public ServiceController(SalonContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateService([FromBody] Service service)
        {
            if (service == null)
            {
                return BadRequest("Service is null.");
            }

            service.Id = Guid.NewGuid();
            _context.Services.Add(service);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, service);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var service = await _context.Services.FindAsync(id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllServices()
        {
            var services = await _context.Services.ToListAsync();
            return Ok(services);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(Guid id, [FromBody] Service service)
        {
            if (service == null || service.Id != id)
            {
                return BadRequest("Service is null or ID mismatch.");
            }

            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null)
            {
                return NotFound();
            }

            existingService.Name = service.Name;

            _context.Services.Update(existingService);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

using Application.ViewModels;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly SalonContext _context;

        public AppointmentsController(SalonContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Appointment), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentViewModel appointmentViewModel)
        {
            if (appointmentViewModel == null)
            {
                return BadRequest("Appointment is null.");
            }

            var userExists = await _context.Users.AnyAsync(u => u.Id == appointmentViewModel.UserId);
            var employeeExists = await _context.Employees.AnyAsync(e => e.Id == appointmentViewModel.EmployeeId);
            var serviceExists = await _context.Services.AnyAsync(s => s.Id == appointmentViewModel.ServiceId);

            if (!userExists || !employeeExists || !serviceExists)
            {
                return BadRequest("Invalid UserId, EmployeeId, or ServiceId.");
            }

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                UserId = appointmentViewModel.UserId,
                EmployeeId = appointmentViewModel.EmployeeId,
                ServiceId = appointmentViewModel.ServiceId,
                AppointmentDate = appointmentViewModel.AppointmentDate
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.Id }, appointment);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Appointment), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Appointment>), 200)]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .ToListAsync();
            return Ok(appointments);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] AppointmentViewModel appointmentViewModel)
        {
            if (appointmentViewModel == null)
            {
                return BadRequest("Appointment is null.");
            }

            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                return NotFound();
            }

            existingAppointment.UserId = appointmentViewModel.UserId;
            existingAppointment.EmployeeId = appointmentViewModel.EmployeeId;
            existingAppointment.ServiceId = appointmentViewModel.ServiceId;
            existingAppointment.AppointmentDate = appointmentViewModel.AppointmentDate;

            _context.Appointments.Update(existingAppointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

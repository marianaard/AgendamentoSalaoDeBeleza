using Application.ViewModels;
using Core.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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

            var appointment = new Appointment
            {
                UserId = appointmentViewModel.UserId,
                EmployeeId = appointmentViewModel.EmployeeId,
                ServiceId = appointmentViewModel.ServiceId,
                AppointmentDate = appointmentViewModel.AppointmentDate
            };

            try
            {
                var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointment);
                return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.Id }, createdAppointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Appointment), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAppointmentById(Guid id)
        {
            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Appointment>), 200)]
        public async Task<IActionResult> GetAllAppointments()
        {
            try
            {
                var appointments = await _appointmentService.GetAllAppointmentsAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

            try
            {
                await _appointmentService.UpdateAppointmentAsync(id, appointmentViewModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            try
            {
                await _appointmentService.DeleteAppointmentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

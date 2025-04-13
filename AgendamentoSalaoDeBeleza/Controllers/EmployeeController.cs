using Core.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoSalaoDeBeleza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee is null.");
            }

            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            if (employee == null || employee.Id != id)
            {
                return BadRequest("Employee is null or ID mismatch.");
            }

            try
            {
                await _employeeService.UpdateEmployeeAsync(id, employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

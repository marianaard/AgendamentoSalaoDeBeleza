using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class EmployeeService
    {
        private readonly SalonContext _context;

        public EmployeeService(SalonContext context)
        {
            _context = context;
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task UpdateEmployeeAsync(Guid id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                throw new Exception("Employee not found.");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;

            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new Exception("Employee not found.");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
    }
}

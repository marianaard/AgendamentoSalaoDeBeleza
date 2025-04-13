using Application.ViewModels;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class AppointmentService
    {
        private readonly SalonContext _context;

        public AppointmentService(SalonContext context)
        {
            _context = context;
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == appointment.UserId);
            var employeeExists = await _context.Employees.AnyAsync(e => e.Id == appointment.EmployeeId);
            var serviceExists = await _context.Services.AnyAsync(s => s.Id == appointment.ServiceId);

            if (!userExists || !employeeExists || !serviceExists)
            {
                throw new Exception("Invalid UserId, EmployeeId, or ServiceId.");
            }

            var conflictingAppointment = await _context.Appointments
                .AnyAsync(a => a.EmployeeId == appointment.EmployeeId && a.AppointmentDate == appointment.AppointmentDate);

            if (conflictingAppointment)
            {
                throw new Exception("The employee already has an appointment at this time.");
            }

            appointment.Id = Guid.NewGuid();
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .ToListAsync();
        }

        public async Task UpdateAppointmentAsync(Guid id, AppointmentViewModel appointmentViewModel)
        {
            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            existingAppointment.UserId = appointmentViewModel.UserId;
            existingAppointment.EmployeeId = appointmentViewModel.EmployeeId;
            existingAppointment.ServiceId = appointmentViewModel.ServiceId;
            existingAppointment.AppointmentDate = appointmentViewModel.AppointmentDate;

            _context.Appointments.Update(existingAppointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentAsync(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}

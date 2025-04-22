using Core.Entities;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UnityTest.Services
{
    public class AppointmentServiceTests
    {
        private readonly DbContextOptions<SalonContext> _dbContextOptions;

        public AppointmentServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<SalonContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateAppointmentAsync_ShouldCreateAppointment_WhenDataIsValid()
        {
            // Arrange
            using var context = new SalonContext(_dbContextOptions);
            var service = new AppointmentService(context);

            var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com", Password = "password" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Test Employee", Email = "employee@example.com" };
            var serviceEntity = new Service { Id = Guid.NewGuid(), Name = "Test Service" };

            context.Users.Add(user);
            context.Employees.Add(employee);
            context.Services.Add(serviceEntity);
            await context.SaveChangesAsync();

            var appointment = new Appointment
            {
                UserId = user.Id,
                EmployeeId = employee.Id,
                ServiceId = serviceEntity.Id,
                AppointmentDate = DateTime.UtcNow.AddHours(1)
            };

            // Act
            var createdAppointment = await service.CreateAppointmentAsync(appointment);

            // Assert
            Assert.NotNull(createdAppointment);
            Assert.Equal(appointment.UserId, createdAppointment.UserId);
            Assert.Equal(appointment.EmployeeId, createdAppointment.EmployeeId);
            Assert.Equal(appointment.ServiceId, createdAppointment.ServiceId);
            Assert.Equal(appointment.AppointmentDate, createdAppointment.AppointmentDate);
        }

        [Fact]
        public async Task CreateAppointmentAsync_ShouldThrowException_WhenEmployeeHasConflictingAppointment()
        {
            // Arrange
            using var context = new SalonContext(_dbContextOptions);
            var service = new AppointmentService(context);

            var employee = new Employee { Id = Guid.NewGuid(), Name = "Test Employee", Email = "employee@example.com" };
            var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com", Password = "password" };
            var serviceEntity = new Service { Id = Guid.NewGuid(), Name = "Test Service" };

            context.Users.Add(user);
            context.Employees.Add(employee);
            context.Services.Add(serviceEntity);
            await context.SaveChangesAsync();

            var appointment1 = new Appointment
            {
                UserId = user.Id,
                EmployeeId = employee.Id,
                ServiceId = serviceEntity.Id,
                AppointmentDate = DateTime.UtcNow.AddHours(1)
            };

            context.Appointments.Add(appointment1);
            await context.SaveChangesAsync();

            var appointment2 = new Appointment
            {
                UserId = user.Id,
                EmployeeId = employee.Id,
                ServiceId = serviceEntity.Id,
                AppointmentDate = appointment1.AppointmentDate
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.CreateAppointmentAsync(appointment2));
            Assert.Equal("The employee already has an appointment at this time.", exception.Message);
        }

        [Fact]
        public async Task CreateAppointmentAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = new SalonContext(_dbContextOptions);
            var service = new AppointmentService(context);

            var employee = new Employee { Id = Guid.NewGuid(), Name = "Test Employee", Email = "employee@example.com" };
            var serviceEntity = new Service { Id = Guid.NewGuid(), Name = "Test Service" };

            context.Employees.Add(employee);
            context.Services.Add(serviceEntity);
            await context.SaveChangesAsync();

            var appointment = new Appointment
            {
                UserId = Guid.NewGuid(),
                EmployeeId = employee.Id,
                ServiceId = serviceEntity.Id,
                AppointmentDate = DateTime.UtcNow.AddHours(1)
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.CreateAppointmentAsync(appointment));
            Assert.Equal("Invalid UserId, EmployeeId, or ServiceId.", exception.Message);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnAppointment_WhenAppointmentExists()
        {
            // Arrange
            using var context = new SalonContext(_dbContextOptions);
            var service = new AppointmentService(context);

            var user = new User { Id = Guid.NewGuid(), Name = "Test User", Email = "test@example.com", Password = "password" };
            var employee = new Employee { Id = Guid.NewGuid(), Name = "Test Employee", Email = "employee@example.com" };
            var serviceEntity = new Service { Id = Guid.NewGuid(), Name = "Test Service" };

            context.Users.Add(user);
            context.Employees.Add(employee);
            context.Services.Add(serviceEntity);
            await context.SaveChangesAsync();

            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                EmployeeId = employee.Id,
                ServiceId = serviceEntity.Id,
                AppointmentDate = DateTime.UtcNow.AddHours(1)
            };

            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetAppointmentByIdAsync(appointment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointment.Id, result.Id);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnNull_WhenAppointmentDoesNotExist()
        {
            // Arrange
            using var context = new SalonContext(_dbContextOptions);
            var service = new AppointmentService(context);

            // Act
            var result = await service.GetAppointmentByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }
    }
}

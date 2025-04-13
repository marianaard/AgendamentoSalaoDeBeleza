using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class ServicesService
    {
        private readonly SalonContext _context;

        public ServicesService(SalonContext context)
        {
            _context = context;
        }

        public async Task<Service> CreateServiceAsync(Service service)
        {
            service.Id = Guid.NewGuid();
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service> GetServiceByIdAsync(Guid id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task UpdateServiceAsync(Guid id, Service service)
        {
            var existingService = await _context.Services.FindAsync(id);
            if (existingService == null)
            {
                throw new Exception("Service not found.");
            }

            existingService.Name = service.Name;

            _context.Services.Update(existingService);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(Guid id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                throw new Exception("Service not found.");
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
        }
    }
}

using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
    }

}

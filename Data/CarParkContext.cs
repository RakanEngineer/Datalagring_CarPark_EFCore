using Datalagring_CarPark_EFCore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Datalagring_CarPark_EFCore.Data
{
    class CarParkContext : DbContext
    {
        public DbSet<Parking> Parking { get; set; }
        private ILoggerFactory Logger { get; }

        public CarParkContext(ILoggerFactory logger)
        {
            Logger = logger;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(Logger);

            optionsBuilder.UseSqlServer("Server=.;Initial Catalog=CarPark;Integrated Security=true; Encrypt=True;Trust Server Certificate=True");
            //Server=(local);Database=CarPark;Trusted_Connection=True
        }
    }
}


using ElectricityApp.Classes;
using Microsoft.EntityFrameworkCore;

namespace ElectricityApp.Models
{
    public class WebDatabaseContext : DbContext
    {
        private IConfiguration _configuration;


        public DbSet<ElectricityModel> electricityDB { get; set; }


        public WebDatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_configuration is not null)
            {
                var connectionString = _configuration.GetConnectionString("WebConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
  
        }
    }
}

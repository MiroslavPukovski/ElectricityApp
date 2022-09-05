using ElectricityApp.Models;
using Microsoft.EntityFrameworkCore;


namespace ElectricityApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Electricity> Electricities { get; set; }
    }
}

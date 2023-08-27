using Microsoft.EntityFrameworkCore;
using WebApi1.Controllers;

namespace WebApi1
{
    public class MyContext:DbContext
    {
        public DbSet<HumidityLog> HumidityLogs { get; set; }
        public DbSet<TemperatureLog> TemperatureLogs { get; set; }
        public DbSet<LoginInfo> LoginInfos { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CustomerDB.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HumidityLog>().HasKey(e => e.Id);
            modelBuilder.Entity<TemperatureLog>().HasKey(e => e.Id);
            modelBuilder.Entity<LoginInfo>().HasKey(e => e.Id);
        }
    }
}

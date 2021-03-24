using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Database
{
    public class PaganodContext : DbContext
    {
        public PaganodContext(DbContextOptions<PaganodContext> dbOptions)
            : base(dbOptions) { }

        public DbSet<WeatherForecast> Forecasts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class WeatherForecast
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}

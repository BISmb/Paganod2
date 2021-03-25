using Microsoft.EntityFrameworkCore;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Context
{
    public partial class PaganodContext : DbContext
    {
        public PaganodContext(DbContextOptions<PaganodContext> dbOptions)
            : base(dbOptions) { }
    }

    public class WeatherForecast : _DataEntity
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}

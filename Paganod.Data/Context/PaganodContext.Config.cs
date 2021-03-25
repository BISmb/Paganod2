using Microsoft.EntityFrameworkCore;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Context
{
    public partial class PaganodContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().HasKey(x => x.Id);

            modelBuilder.HasDefaultSchema("paganod");

            foreach (var dbSetType in GetConfigurableTypes())
            {
                var entityBuilder = modelBuilder.Entity(dbSetType);
                var dbSetTypeInstance = Activator.CreateInstance(dbSetType);
                ((_DataEntity)dbSetTypeInstance).AddEfConfig(dbSetType, ref entityBuilder, out string strTableName);
            }

            base.OnModelCreating(modelBuilder);
        }

        protected Type[] GetConfigurableTypes()
        {
            // TODO: Change this to look at whole solution instead of just the properties listed on the Paganod Context

            var dbSetTypes = GetType().GetProperties()
                .Where(x => x.PropertyType.GenericTypeArguments.Length > 0)
                .Select(x => x.PropertyType.GenericTypeArguments.First());

            return dbSetTypes.ToArray();
        }
    }
}

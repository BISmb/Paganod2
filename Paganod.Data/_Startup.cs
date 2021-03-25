using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Paganod.Data.Context;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data
{
    public static class _Startup
    {
        public static IServiceCollection AddPaganodDatabase(this IServiceCollection services)
        {
            ConnectionProperties connProperties = new ConnectionProperties
            {
                ServerName = "127.0.0.1",
                DatabaseEngine = "Mysql",
                DatabaseName = "paganod2",
                User = "root",
                Password = "root"
            };

            services.AddDbContext<PaganodContext>(opt =>
            {
                opt.UseMySql($"{connProperties}", ServerVersion.FromString("8.0.21"), dbOpt =>
                {
                    dbOpt.SchemaBehavior(MySqlSchemaBehavior.Translate,
                        new MySqlSchemaNameTranslator((schemaName, objName) => $"{schemaName}_{objName}"));
                });
            });
            return services;
        }
    }
}

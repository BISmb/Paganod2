using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Context
{
    public partial class PaganodContext
    {
        public IDbConnection GetNewDbConnection(bool open = false)
        {
            IDbConnection dbConnection = new DatabaseFacade(this).GetDbConnection();
            Type dbConnectionType = dbConnection.GetType();
            IDbConnection newDbConnection = Activator.CreateInstance(dbConnectionType) as IDbConnection;

            string connectionString = Database.GetConnectionString();
            newDbConnection.ConnectionString = connectionString;

            return newDbConnection;
        }
    }
}

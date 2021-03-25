using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Paganod.Data
{
    public class ConnectionProperties
    {
        public string ServerName { get; set; }
        public string DatabaseEngine { get; set; }
        public string DatabaseName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }


        public ConnectionProperties() { }

        public ConnectionProperties(string connectionString)
        {
            Dictionary<string, string> connInfo = new Dictionary<string, string>();

            foreach (var part in connectionString.Split(";"))
                connInfo.Add(part.Split("=")[0], part.Split("=")[1]);

            DatabaseEngine = "mysql";
            ServerName = connInfo["Server"];
            DatabaseName = connInfo["Database"];

            User = connInfo["User Id"];
            Password = connInfo["Password"];
        }

        public override string ToString()
        {
            return DatabaseEngine.ToLower() switch
            {
                "mysql" => ConstructMySqlString(),
                "sqlserver" => ConstructSqlServerString(),
                //Constants.Database.Engine.SqlLite => ConstructSqlLiteString(),
                //Constants.Database.Engine.SqlLiteInMemory => ConstructSqlLiteString(true),

                _ => throw new NotImplementedException()
            };
        }

        private string ConstructSqlLiteString(bool inMemory = false)
        {
            if (inMemory)
                return $":memory:";

            return $"";
        }

        private string ConstructSqlServerString()
        {
            throw new NotImplementedException();
        }

        private string ConstructMySqlString()
        {
            return $"Server={ServerName};Database={DatabaseName};User Id={User};Password={Password};AllowUserVariables=True;Use Affected Rows=False";
        }
    }
}

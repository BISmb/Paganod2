using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Generators;
using FluentMigrator.Runner.Generators.MySql;
using FluentMigrator.Runner.Generators.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Npgsql;
using Paganod.Shared;
using Paganod.Shared.Type;
using Paganod.Sql.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Utility
{
    public static class IDbExtensions
    {
        public static IDbConnection EnsureOpen(this IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            return dbConnection;
        }

        public static IMigrationRunner GetMigrationRunner(this IDbConnection dbConection)
        {
            //string dbType = connectionProperties.DbType;
            //string dbName = connectionProperties.DbName;
            //string serverName = connectionProperties.ServerName;
            //connectionProperties.AdditionalOptions.Add("Allow User Variables", "true");

            var serviceCollection = new ServiceCollection()
                .AddFluentMigratorCore()
                //.AddScoped<MySqlQuoter, _MySqlQuoter>()
                //.AddScoped<SQLiteQuoter, _SqlLiteQuoter>()
                .AddLogging(log => log.AddFluentMigratorConsole());

            serviceCollection.ConfigureRunner(runner => {
                runner.AddDbType("MySql" /*dbConection.GetDatabaseEngineType()*/);
                runner.WithGlobalConnectionString(dbConection.ConnectionString);
                //runner.ScanIn(typeof(__VersionTable).Assembly).For.Migrations();
            });


            //var result = System.Reflection.Assembly.Load(typeof(FluentMigrator.Runner.Generators.MySql.MySql4Generator).Assembly.GetName());
            //Console.WriteLine(result.FullName);
            //var typeMap = result.DefinedTypes.Where(dt => (typeof(FluentMigrator.Runner.Generators.Base.TypeMapBase).IsAssignableFrom(dt.AsType())));
            //typeMap.Dump();


            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider.GetRequiredService<IMigrationRunner>();
        }

        public static IEnumerable<Record> RunCommand(this IDbConnection dbConnection, string sql, params object[] Parameters)
        {
            IList<Record> results = new List<Record>();

            var command = dbConnection.CreateCommand(sql, Parameters);

            using(command)
                results = command.GetResults();

            return results;
        }

        public static IDbCommand CreateCommand(this IDbConnection dbConnection, string sql, params object[] Parameters)
        {
            IDbCommand command = dbConnection.CreateCommand();
            command.CommandText = sql;
            command.Connection = dbConnection;

            int i = 0;
            foreach (object obj in Parameters)
            {
                string paramName = $"@p{i}";
                var dbParam = command.CreateParameter();
                dbParam.ParameterName = paramName;
                dbParam.Value = obj;
                command.Parameters.Add(dbParam);
                // TODO: Set DbType
                command.CommandText = command.CommandText.ReplaceFirstOccuranceOf("{p}", paramName);
                i++;
            }

            return command;
        }

        public static string ReplaceFirstOccuranceOf(this string str, string StringToReplace, string newStringValue)
        {
            int startIndex = str.IndexOf(StringToReplace);
            str = str.Insert(startIndex, newStringValue);


            return str.Remove(startIndex + newStringValue.Length, StringToReplace.Length);
        }

        public static SqlOptions GetSqlOptions(this IDbConnection db)
        {
            switch (db)
            {
                case SqlConnection s:
                default:
                    return new SqlOptions("SqlServer", '[', ']', "dbo");

                case NpgsqlConnection p:
                    return new SqlOptions("Postgres", '"', '"', "public");

                case MySqlConnection m:
                    return new SqlOptions("MySql", '`', '`');

                case SqliteConnection sl:
                    return new SqlOptions("Sqlite", ' ', ' ');
            }
        }

        public static IList<Record> GetRecords(this IDbCommand dbCommand, string strRecordType = "Query") => dbCommand.GetResults(strRecordType);

        public static IList<Record> GetResults(this IDbCommand dbCommand, string strRecordType = "Query")
        {
            IList<Record> results = new List<Record>();

            dbCommand.Connection.EnsureOpen();

            using (var dbReader = dbCommand.ExecuteReader())
            {
                while (dbReader.Read())
                {
                    var record = new Record(strRecordType);

                    for (int i = 0; i < dbReader.FieldCount; i++)
                        record[dbReader.GetName(i)] = dbReader.GetValue(i);

                    results.Add(record);
                }
            }

            dbCommand.Connection.Close();

            return results;
        }

        public static IMigrationRunnerBuilder AddDbType(this IMigrationRunnerBuilder runnerBuilder, string dbType = "")
        {
            return dbType switch
            {
                //Constants.Database.Engine.SqlServer => runnerBuilder.AddSqlServer2016(),
                "MySql" => runnerBuilder.AddMySql5(),
                //Constants.Database.Engine.SqlLite => runnerBuilder.AddSQLite(),

                _ => throw new Exception(),
            };
        }

        public static string GetDatabaseEngineType(this IDbConnection dbConnection)
        {
            return dbConnection switch
            {
                MySqlConnection => "mysql",
                SqlServerConnection => "sqlserver",

                _ => throw new Exception(),
            };
        }
    }
}

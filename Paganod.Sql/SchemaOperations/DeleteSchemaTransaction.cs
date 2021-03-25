using FluentMigrator;
using Paganod.Sql.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.SchemaOperations
{
    public class DeleteSchemaTransaction : IDisposable
    {
        private IDbConnection _Db { get; set; }
        private DeleteMigaration _SchemaMigration { get; }

        public DeleteSchemaTransaction(IDbConnection newDbConnection, string TableName)
        {
            _Db = newDbConnection;
            _SchemaMigration = new DeleteMigaration(TableName);
        }

        public async Task RunAsync()
        {
            try
            {
                var migrationRunner = _Db.GetMigrationRunner();
                await Task.Run(() => migrationRunner.Up(_SchemaMigration));

                //return new ModifySchemaResponse(true, $"{_SchemaModelName} created successfully");
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            ////_Db.Dispose();
            //_SchemaMigration = null;
        }
    }

    internal class DeleteMigaration : Migration
    {
        public string TableName { get; }

        public DeleteMigaration(string strTableName)
        {
            TableName = strTableName;
        }

        public override void Down() => throw new NotImplementedException();

        public override void Up()
        {
            // Need to Query for Any Contraints

            // Make sure table is empty or user override


            Delete.Table(TableName);
        }
    }
}

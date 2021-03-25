using Paganod.Sql.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Methods
{
    public static class DeleteMethods
    {
        public static int Delete(this IDbConnection dbConnection, string RecordType, object RecordId)
        {
            var tableMap = _SQL.GetTableMap(RecordType: RecordType);
            string TABLE = tableMap.GetFullTableName(dbConnection.GetSqlOptions());
            string PrimaryKeyProperty = tableMap.Columns.First(x => x.IsPrimaryKey).ColumnName;

            using(dbConnection.EnsureOpen())
            {
                using(var command = dbConnection.CreateCommand())
                {
                    var p = command.CreateParameter();
                    p.ParameterName = "@Id";
                    p.Value = RecordId;
                    command.Parameters.Add(p);

                    command.CommandText = $"DELETE FROM {TABLE} WHERE {PrimaryKeyProperty} = @Id;";

                    int rows = command.ExecuteNonQuery();

                    return rows;
                }
            }
        }
    }
}

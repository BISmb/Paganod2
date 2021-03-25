using Paganod.Shared.Type;
using Paganod.Sql.Types;
using Paganod.Sql.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Methods
{
    public static class UpdateMethods
    {
        public static Record Update(this IDbConnection dbConnection, Record recordToUpdate)
        {
            TableMap tableMap = _SQL.GetTableMap(RecordType: recordToUpdate.Type);
            string PrimaryKeyProperty = tableMap.Columns.First(x => x.IsPrimaryKey).ColumnName;

            var command = dbConnection.CreateCommand();

            string UPDATE = $"UPDATE {tableMap.GetFullTableName(dbConnection.GetSqlOptions())}";
            string OUTPUT = $"OUTPUT INSERTED.{PrimaryKeyProperty} AS {PrimaryKeyProperty.ToLower()}";
            string COLUMNS = $"({string.Join(", ", recordToUpdate.Data.Keys)})";
            string VALUES = "";

            var oPrimaryKeyValue = recordToUpdate[PrimaryKeyProperty];

            Dictionary<string, object> dicValuesToUpdate = new(recordToUpdate.Data, StringComparer.OrdinalIgnoreCase);

            dicValuesToUpdate.Remove(PrimaryKeyProperty.ToLower());
            dicValuesToUpdate.RemoveAll(x => x.Value == null);

            for (int i = 0; i < dicValuesToUpdate.Keys.Count; i++)
            {
                KeyValuePair<string, object> p = dicValuesToUpdate.ElementAt(i);

                //if (p.Key.ToLower() == PrimaryKeyProperty.ToLower())
                //    continue;

                //if (p.Value == null)
                //    continue;

                string paramName = $"@{p.Key}{i}";

                VALUES += $"{p.Key} = {paramName}";

                if (i < (dicValuesToUpdate.Keys.Count - 1))
                    VALUES += ",";

                var sqlParam = command.CreateParameter();

                ColumnMap columnMap = tableMap.Columns.FirstOrDefault(x => x.ColumnName.ToLower() == p.Key.ToLower());
                if (columnMap.Conversion != null)
                    sqlParam.Value = columnMap.Conversion.AppToDbConvert(p.Value);
                else if (p.Value is Guid)
                    sqlParam.Value = p.Value.ToString();
                else
                    sqlParam.Value = p.Value;

                sqlParam.ParameterName = paramName;
                command.Parameters.Add(sqlParam);

                //int numOfActualCols = dicValuesToUpdate.Keys.Count - 1; // -1 because of array

                //if (i == numOfActualCols)
                //    VALUES = VALUES.Remove(VALUES.Length - 1, 1);
            }

            var sqlParamId = command.CreateParameter();
            sqlParamId.Value = oPrimaryKeyValue;
            sqlParamId.ParameterName = $"@Id";
            command.Parameters.Add(sqlParamId);

            command.CommandText = $"{UPDATE} SET {VALUES} WHERE {PrimaryKeyProperty} = @Id;";
            int rows = command.ExecuteNonQuery();

            if (rows < 1)
                throw new Exception("No rows were affected in update");

            var record = dbConnection.Get(recordToUpdate.Type, oPrimaryKeyValue);
            return record;
        }
    }
}

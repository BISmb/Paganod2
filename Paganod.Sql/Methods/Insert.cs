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
    public static class InsertMethods
    {
        public static object Insert(this IDbConnection dbConnection, Record recordToInsert)
        {
            Guid insertedRecordId = Guid.Empty;
            SqlOptions sqlOpttions = dbConnection.GetSqlOptions();
            TableMap tableMap = _SQL.GetTableMap(RecordType: recordToInsert.Type);
            string PrimaryKeyProperty = tableMap.Columns.First(x => x.IsPrimaryKey).ColumnName;

            var command = dbConnection.CreateCommand();

            //if (sqlOpttions.ProviderType.ToLower() == "sqlite")
            //{
                insertedRecordId = Guid.NewGuid();
                recordToInsert.Data.Add(PrimaryKeyProperty, insertedRecordId);
            //}

            string INSERT = $"INSERT INTO {tableMap.GetFullTableName(sqlOpttions)}";
            string OUTPUT = $"OUTPUT INSERTED.{PrimaryKeyProperty} AS {PrimaryKeyProperty.ToLower()}";

            IList<string> colNames = recordToInsert.Data.Keys.ToList();
            for (int i = 0; i < colNames.Count; i++)
                colNames[i] = colNames[i]; // TODO: ToSQLString()

            string COLUMNS = $"({string.Join(", ", colNames)})";
            string VALUES = "";

            int paramCount = 0;
            string paramName = "";

            foreach (KeyValuePair<string, object> p in recordToInsert.Data)
            {
                paramCount++;
                paramName = p.Key; // TODO: .ToSqlString();
                VALUES += $"@{paramName}{paramCount},";

                var sqlParam = command.CreateParameter();

                IConversion iConversion = null;
                if (tableMap.Columns.Any(x => x.ColumnName.ToLower() == paramName))
                    if (tableMap.Columns.First(x => x.ColumnName.ToLower() == paramName).Conversion != null)
                        iConversion = tableMap.Columns.First(x => x.ColumnName.ToLower() == paramName).Conversion;

                if (iConversion is not null) 
                    sqlParam.Value = iConversion.AppToDbConvert(p.Value);
                else 
                    sqlParam.Value = p.Value;

                sqlParam.ParameterName = $"@{paramName}{paramCount}";
                command.Parameters.Add(sqlParam);

                if (paramCount == recordToInsert.Data.Keys.Count)
                    VALUES = VALUES.Remove(VALUES.Length - 1, 1);
            }

            command.CommandText = $"{INSERT} {COLUMNS} VALUES ({VALUES});";

            //if (sqlOpttions.ProviderType.ToLower() == "sqlite")
            //{
            //    command.CommandText = $"{INSERT} {COLUMNS} VALUES ({VALUES});";
            //    command.ExecuteNonQuery();

            //    recordToInsert[PrimaryKeyProperty] = insertedRecordId;
            //    return recordToInsert; // TODO: Use Get()
            //}
            //else
            //{
            //    switch (sqlOpttions.ProviderType.ToLower())
            //    {
            //        case "sqlserver":
            //            command.CommandText = $"{INSERT} {COLUMNS} {OUTPUT} VALUES ({VALUES});";
            //            break;

            //        case "mysql":
            //            //command.CommandText = $"{INSERT} {COLUMNS} VALUES ({VALUES}); SELECT @last_uuid;";
            //            command.CommandText = $"{INSERT} {COLUMNS} VALUES ({VALUES}); SELECT {PrimaryKeyProperty};";
            //            break;

            //        default:
            //            command.CommandText = $"{INSERT} {COLUMNS} VALUES ({VALUES});";
            //            break;
            //    }


            int intRowsAffected = command.ExecuteNonQuery();
            if (intRowsAffected < 1) throw new Exception();
            return insertedRecordId;
        }
    }
}

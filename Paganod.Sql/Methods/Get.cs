using Dapper;
using DynamicODataToSQL;
using Microsoft.Data.Sqlite;
using MySqlConnector;
using Npgsql;
using Paganod.Shared.Type;
using Paganod.Shared.Type.Query;
using Paganod.Sql.Types;
using Paganod.Sql.Utility;
using SqlKata.Compilers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Methods
{
    public static class GetMethods
    {
        private static ODataToSqlConverter _oDataSqlConvertor { get; set; }

        //public static bool Exists(this IDbConnection dbConnection, string strRecordType, object tRecordId)
        //{
        //    var getResult = dbConnection.Get(strRecordType, tRecordId);
        //    return !(getResult..Count == 0);
        //}

        public static Record Get(this IDbConnection dbConnection, string RecordType, object Id, string schemaOwner = "")
        {
            TableMap tableMap = _SQL.GetTableMap(RecordType: RecordType);

            string FullTableName = tableMap.GetFullTableName(dbConnection.GetSqlOptions());

            var PrimaryKeyProperty = tableMap.Columns.First(x => x.IsPrimaryKey).ColumnName;
            var columns = tableMap.Columns.Where(c => !c.IsDatabaseGenerated).Select(x => x.ColumnName).ToList();
            if (!columns.Contains(PrimaryKeyProperty)) columns.Add(PrimaryKeyProperty);
            var AllColumns = string.Join(',', columns);

            string sql = $"SELECT {AllColumns} FROM {FullTableName} WHERE {PrimaryKeyProperty} = @Id";

            Dictionary<string, object> values = QueryFirst<Dictionary<string, object>>(dbConnection, sql, new { Id });

            return new Record(tableMap.RecordType, values);
        }

        public static IEnumerable<Record> GetRecords(this IDbConnection dbConnection, ODataQuery oQuery)
        {
            //QueryResults oQueryResults = new();
            TableMap tableMap = null;
            List<Record> lstRecords = new();

            try
            {
                using (dbConnection.EnsureOpen())
                {
                    tableMap = _SQL.GetTableMap(TableName: oQuery.BaseTableName);

                    if (oQuery.SelectFields is null)
                        oQuery.Select(tableMap.Columns.Where(c => !c.IsDatabaseGenerated).Select(x => x.ColumnName).ToArray());

                    if (!oQuery.SelectFields.Contains(tableMap.Columns.First(x => x.IsPrimaryKey).ColumnName))
                        oQuery.Select(tableMap.Columns.First(x => x.IsPrimaryKey).ColumnName);

                    _oDataSqlConvertor ??= new ODataToSqlConverter(new EdmModelBuilder(), dbConnection.GetKataSqlCompiler());

                    /*
                    * Get Count with Paging
                    */
                    //oQueryResults.TotalItems = GetResultCount(dbConnection, oQuery);

                    var result = _oDataSqlConvertor.ConvertToSQL(
                            oQuery.BaseTableName,
                            oQuery.GetDynamicDictionary(),
                            false);

                    string sql = result.Item1;
                    IDictionary<string, object> sqlParams = result.Item2;

                    using (IDbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = result.Item1;

                        foreach (var p in result.Item2)
                        {
                            var dbParam = dbCommand.CreateParameter();
                            dbParam.ParameterName = p.Key;
                            dbParam.Value = p.Value;
                            dbCommand.Parameters.Add(dbParam);
                        }

                        lstRecords = dbCommand.GetRecords(tableMap.RecordType).ToList();
                    }
                }

                return lstRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static long GetResultCount(IDbConnection dbConnection, ODataQuery oQuery)
        {
            try
            {
                ODataQuery oDataQueryWithoutPaging = oQuery;
                oDataQueryWithoutPaging.Top = 0;
                oDataQueryWithoutPaging.Count = true;
                var querySqlWithoutPaging = _oDataSqlConvertor.ConvertToSQL(
                      oQuery.BaseTableName,
                      oQuery.GetDynamicDictionary(),
                      false);

                string sql = querySqlWithoutPaging.Item1;

                int intSubStringStartIndex = sql.IndexOf("SELECT") + "SELECT".Length;
                string strSqlReplace = sql.Substring(intSubStringStartIndex, sql.IndexOf("FROM") - intSubStringStartIndex).Trim();
                sql = sql.Replace(strSqlReplace, "COUNT(*)");

                IDictionary<string, object> sqlParams = querySqlWithoutPaging.Item2;

                //using (dbConnection.EnsureOpen())
                //{
                    using (IDbCommand dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.CommandText = sql;

                        foreach (var p in sqlParams)
                        {
                            var dbParam = dbCommand.CreateParameter();
                            dbParam.ParameterName = p.Key;
                            dbParam.Value = p.Value;
                            dbCommand.Parameters.Add(dbParam);
                        }

                        long intResultCount = (long)dbCommand.ExecuteScalar();

                        return intResultCount;
                    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static Compiler GetKataSqlCompiler(this IDbConnection dbConnection)
        {
            return dbConnection switch
            {
                MySqlConnection => new MySqlCompiler(),
                SqlConnection => new SqlServerCompiler(),
                NpgsqlConnection => new PostgresCompiler(),
                SqliteConnection => new SqliteCompiler(),

                _ => throw new NotImplementedException()
            };
        }

        private static Dictionary<string, object> QueryFirst<TDict>(this IDbConnection dbConnection, string sql, object parameters = null)
            where TDict : IDictionary<string, object>
        {
            var values = dbConnection.Query(sql, parameters);
            var elements = values.ToList().First();

            IDictionary<string, object> dapperRowProperties = elements as IDictionary<string, object>;
            Dictionary<string, object> dicElements = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> property in dapperRowProperties)
                dicElements.Add(property.Key, property.Value);

            return dicElements;
        }
    }
}

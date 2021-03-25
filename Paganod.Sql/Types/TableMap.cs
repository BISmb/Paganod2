using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Types
{
    public class TableMap
    {
        public Type ObjectType { get; set; }
        public string RecordType { get; set; }
        public string TableName { get; set; }
        public string SchemaOwner { get; set; }
        public IList<ColumnMap> Columns { get; set; } = new List<ColumnMap>();

        //public TableMap(Type newObjectType, string newSchemaOwner = "", string overrideTableName = "", IEnumerable<ColumnMap> _columnMaps = null)
        //{
        //    ObjectType = newObjectType;
        //    TableName = newObjectType.Name;

        //    if (!string.IsNullOrEmpty(overrideTableName))
        //        TableName = overrideTableName;

        //    if (_columnMaps == null)
        //    {
        //        IEnumerable<string> propertyNames = ObjectType.GetProperties().Where(x => x.PropertyType.IsValueType || x.PropertyType == typeof(string)).Select(x => x.Name);
        //        foreach (var e in propertyNames)
        //            Columns.Add(new ColumnMap(e, false, false, false));
        //    }
        //    else
        //    {
        //        foreach (var c in _columnMaps)
        //            Columns.Add(c);
        //    }

        //    SchemaOwner = newSchemaOwner;

        //    if (ObjectType != null)
        //        RecordType = ObjectType.Name;
        //}

        public TableMap(string newTableName, IList<ColumnMap> _columnMaps, string newRecordType = "", string newSchemaOwner = "")
        {
            TableName = newTableName;

            foreach (var col in _columnMaps)
                Columns.Add(col);

            SchemaOwner = newSchemaOwner;
            RecordType = newRecordType;
        }

        public ColumnMap Property(string columnName)
        {
            return Columns.FirstOrDefault(x => x.ColumnName == columnName);
        }

        public void HasKey(string colName, bool IsGenerated = false)
        {
            Columns.First(x => x.ColumnName == colName).PrimaryKey(IsGenerated);
        }

        public void Ignore(string colName)
        {
            // TODO: Ignore or Remove?

            Columns.Remove(Columns.First(x => x.ColumnName == colName));
        }

        internal string GetFullTableName(SqlOptions sqlOptions, string schemaOwner = "")
        {
            if (!sqlOptions.SupportsSchemaOwner)
            {
                return $"{sqlOptions.LeftSeperator}{TableName}{sqlOptions.RightSeperator}";
            }
            else
            {
                if (string.IsNullOrEmpty(schemaOwner))
                    return $"{sqlOptions.LeftSeperator}{SchemaOwner}{sqlOptions.RightSeperator}.{sqlOptions.LeftSeperator}{TableName}{sqlOptions.RightSeperator}";
                else
                    return $"{sqlOptions.LeftSeperator}{sqlOptions.DefaultSchema}{sqlOptions.RightSeperator}.{sqlOptions.LeftSeperator}{TableName}{sqlOptions.RightSeperator}";
            }
        }

        public TableMap HasTableName(string tableName)
        {
            TableName = tableName;
            return this;
        }
        public TableMap InSchema(string schemaName)
        {
            SchemaOwner = schemaName;
            return this;
        }
    }
}

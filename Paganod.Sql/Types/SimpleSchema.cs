using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Types
{
    public class SimpleSchemaModel
    {
        public int TempId { get; set; }
        public string RecordType { get; set; }
        public string TableName { get; set; }
        public string PrimaryKeyColumnName { get; set; }
        public List<SimpleSchemaColumn> Columns { get; set; } = new();
        public List<SimpleSchemaRelation> Relations { get; set; } = new();
    }

    public class SimpleSchemaColumn
    {
        public int TempId { get; set; }
        public string Name { get; set; }
        public DbType Type { get; set; }
        public bool IsUnique { get; set; }
        public bool IsRequired { get; set; }
        public IDictionary<string, string> TypeOptions { get; set; }

        public SimpleSchemaColumn()
        {
            TypeOptions = new Dictionary<string, string>();
        }
    }

    public class SimpleSchemaRelation
    {
        public int SchemaRelationId { get; set; }
        public string ReferencedSchemaPrimaryKeyColumnName { get; set; }
        public string PrincipalSchemaPrimaryKeyColumnName { get; set; }
        public Enums.Data.RelationshipType RelationshipType { get; set; }
        public string RelatedSchemaName { get; set; }
    }

    //public enum RelationshipType
    //{
    //    OneToOne,
    //    OneToMany,
    //    ManyToMany,
    //    ManyToOne
    //}
}

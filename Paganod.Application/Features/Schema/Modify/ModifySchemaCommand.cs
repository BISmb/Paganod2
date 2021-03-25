using MediatR;
using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Paganod.Application.Features.Schema.Modify
{
    public partial class ModifySchemaCommand : IRequest<Result<int>>
    {
        public int SchemaModelId { get; set; }
        public string SchemaModelPKName { get; set; }
        public DbType SchemaModelPKType { get; set; }
        public string SingularName { get; set; }
        public string PluralName { get; set; }

        public class SchemaColumn
        {
            public int SchemaColumnId { get; set; }
            public string ColumnName { get; set; }
            public DbType ColumnType { get; set; }
        }

        public class Relationship
        {
            public int RelationshipId { get; set; }
            public Enums.Data.RelationshipType Type { get; set; }
            public string DisplayName { get; set; }
        }

        public class ColumnProperty
        {
            public int ColumnPropertyId { get; set; }
            public int SchemaColumnId { get; set; }
            public string ProperyName { get; set; }
            public string PropertyValue { get; set; }
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paganod.Shared;
using Paganod.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class SchemaRelationship : _DataEntity
    {
        public Guid SchemaRelationId { get; set; }
        public Enums.Data.RelationshipType RelationshipType { get; set; }
        public string PrincipalSchemaPrimaryKeyColumnName { get; set; }
        public Guid PrincipalSchemaId { get; set; }
        public string PrincipalSchemaName { get; set; }
        public Guid RelatedSchemaId { get; set; }
        public string RelatedSchemaName { get; set; }
        public string RelatedSchemaPrimaryKeyColumnName { get; set; }

        public override void AddEfConfig(Type typeEntity, ref EntityTypeBuilder oEntityTypeBuilder, out string strTableName)
        {
            oEntityTypeBuilder.HasKey(nameof(SchemaRelationId));

            base.AddEfConfig(typeEntity, ref oEntityTypeBuilder, out strTableName);
        }
    }
}

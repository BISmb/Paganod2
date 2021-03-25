using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Paganod.Data.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class SchemaModel : _DataEntity
    {
        public Guid SchemaModelId { get; set; }
        public string TableName { get; set; }
        public string TableDisplayName { get; set; }
        public string RecordName { get; set; }
        public string RecordDisplayName { get; set; }
        public string PrimaryKeyColumnName { get; set; }

        public List<SchemaColumn> Columns { get; set; } = new();

        public List<SchemaRelationship> Relations { get; set; } = new();

        public override void AddEfConfig(Type typeEntity, ref EntityTypeBuilder oEntityTypeBuilder, out string strTableName)
        {
            oEntityTypeBuilder
                .HasMany(typeof(SchemaColumn), nameof(Columns))
                .WithOne()
                .HasForeignKey(nameof(SchemaModelId));

            oEntityTypeBuilder.Ignore(nameof(Relations));

            base.AddEfConfig(typeEntity, ref oEntityTypeBuilder, out strTableName);
        }
    }
}

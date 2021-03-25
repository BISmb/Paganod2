using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Paganod.Shared;
using Paganod.Data.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class SchemaColumn : _DataEntity
    {
        public Guid SchemaColumnId { get; set; }
        public Guid SchemaModelId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DbType Type { get; set; }
        public bool IsRequired { get; set; }
        public string OptionsJson { get; set; }
    }
}

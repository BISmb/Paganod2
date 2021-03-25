using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Paganod.Shared;
using Paganod.Shared.Type.Query;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class RecordView : _DataEntity
    {
        public Guid RecordViewId { get; set; }
        public Guid SchemaModelId { get; set; }
        public string Name { get; set; }
        public string QueryAsString { get; set; }

        public RecordView() { }

        public RecordView(Guid lngSchemaModelId, string strName, ODataQuery oQuery = null)
        {
            SchemaModelId = lngSchemaModelId;
            Name = strName;
            QueryAsString = JsonSerializer.Serialize(oQuery);
        }
    }
}

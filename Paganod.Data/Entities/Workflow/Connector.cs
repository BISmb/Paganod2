using Paganod.Shared;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Connector : _DataEntity
    {
        //public Guid ConnectorId { get; set; }
        public Enums.Workflow.ConnectorType Type { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
    }
}

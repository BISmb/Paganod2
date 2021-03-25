using Paganod.Shared;
using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Trigger : _DataEntity
    {
        //public Guid TriggerId { get; set; }
        public Enums.Workflow.TriggerType Type { get; set; }
        public string SubType { get; set; }
    }
}

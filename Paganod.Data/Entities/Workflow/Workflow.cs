using Paganod.Shared;
using Paganod.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Workflow : _DataEntity
    {
        //public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public IList<Process> Processes { get; set; } = new List<Process>();
    }
}

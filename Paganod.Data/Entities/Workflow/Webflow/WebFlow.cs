using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class WebFlow : _DataEntity
    {
        //public Guid WebFlowId { get; set; }
        public Guid ScriptId { get; set; }
        public Guid FinishedProcessId { get; set; }
    }
}

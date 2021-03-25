using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class ApiTrigger : _DataEntity
    {
        //public Guid ApiTriggerId { get; set; }
        public string RegisteredRoute { get; set; }
        public Guid PluginId { get; set; }
        public string PluginMethod { get; set; }
    }
}

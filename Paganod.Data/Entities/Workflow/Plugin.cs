using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Plugin : _DataEntity
    {
        //public Guid PluginId { get; set; }
        public string PluginName { get; set; }
        public byte[] PluginReference { get; set; }
    }
}

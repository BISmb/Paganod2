using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Script : _DataEntity
    {
        //public Guid ScriptId { get; set; }
        public string LocationType { get; set; } // Will be enum later on
        public string Language { get; set; } // Will be enum later on
        public string Value { get; set; }
    }
}

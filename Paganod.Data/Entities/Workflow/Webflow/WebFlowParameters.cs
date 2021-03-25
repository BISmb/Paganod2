using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class WebFlowParameters : _DataEntity
    {
        //public Guid WebFlowParameterId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

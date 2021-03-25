using Paganod.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class SolutionItem : _DataEntity
    {
        public Guid SolutionItemId { get; set; }
        public int SolutionId { get; set; }
        public string ComponentType { get; set; }
        public int ComponentId { get; set; }
    }
}

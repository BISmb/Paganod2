using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class Solution : _DataEntity
    {
        public string Name { get; set; }
        public bool IsConfigurable { get; set; }
        public string Version { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string InstalledBy { get; set; }
        public DateTime InstalledOn { get; set; }
    }
}

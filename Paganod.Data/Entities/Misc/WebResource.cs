using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class WebResource : _DataEntity
    {
        public Guid WebResourceId { get; set; }
        public string ShortName { get; set; }
        public string PreviewImage { get; set; } // Src link (or base 64 if embedded)
        public string Content { get; set; }
    }
}

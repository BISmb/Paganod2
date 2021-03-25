using Paganod.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class FileLink : _DataEntity
    {
        public Guid FileLinkId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; } // Eventually will be enum
        public string Link { get; set; }

        public string RecordType { get; set; }
        public Guid RecordId { get; set; }
    }
}

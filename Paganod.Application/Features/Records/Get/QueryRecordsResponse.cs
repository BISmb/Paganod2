using Paganod.Shared.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Application.Features.Records.Get
{
    public class QueryRecordsResponse
    {
        public IEnumerable<Record> Records { get; set; }
    }
}

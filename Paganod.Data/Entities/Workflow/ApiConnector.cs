using Paganod.Shared;
using Paganod.Data.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Data.Entities
{
    public class ApiConnector : _DataEntity
    {
        //public Guid ApiConnectorId { get; set; }
        public Guid ConnectorId { get; set; }
        public string BaseUrl { get; set; }
        public string Version { get; set; }
        public string OpenAPISpec { get; set; }
    }
}

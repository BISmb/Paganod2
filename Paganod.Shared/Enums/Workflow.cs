using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Shared
{
    public static partial class Enums
    {
        public static class Workflow
        {
            public enum FlowType
            {
                HTTP,
                SYSTEM
            }

            public enum ConnectorType
            {
                API,
                FILE,
                DATABASE,
                SYSTEM,
            }

            public enum TriggerType
            {
                SchemaChange,
                Record,
                ApiRequest,
                Connector
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Types
{
    public class SqlOptions
    {
        public string ProviderType { get; set; }
        public bool SupportsSchemaOwner { get; set; } = false;
        public string DefaultSchema { get; set; } // TODO: Some databases have different schema (PostGres / SQL Server)
        public char LeftSeperator { get; set; }
        public char RightSeperator { get; set; }

        public SqlOptions() { }

        public SqlOptions(string newProviderType, char leftSeperator, char rightSeperator, string defaultSchema = "")
        {
            ProviderType = newProviderType;
            LeftSeperator = leftSeperator;
            RightSeperator = rightSeperator;

            DefaultSchema = defaultSchema;

            if (!string.IsNullOrEmpty(DefaultSchema))
                SupportsSchemaOwner = true;
        }
    }
}

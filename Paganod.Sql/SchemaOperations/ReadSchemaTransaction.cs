using Paganod.Sql.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.SchemaOperations
{
    public class ReadExistingSchemaTransaction : IDisposable
    {
        private IDbConnection _DbConnection { get; set; }

        public ReadExistingSchemaTransaction(IDbConnection dbConnection)
        {
            _DbConnection = dbConnection;
        }

        //public IEnumerable<SchemaModel> GetExistingSchema()
        //{
        //    var metadataReader = SqlMetaDataFactory.CreateReader(_DbConnection);
        //    return metadataReader.GetExistingSchema();
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

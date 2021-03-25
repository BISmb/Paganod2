using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Sql.Types
{
    public class ColumnMap
    {
        public string ColumnName { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsKey { get; set; }
        public bool IsDatabaseGenerated { get; set; }

        public IConversion Conversion { get; set; }

        public ColumnMap(string newColumnname, bool isDatabaseGenerated = false, bool isKey = false, bool isPrimaryKey = false)
        {
            ColumnName = newColumnname;
            IsDatabaseGenerated = isDatabaseGenerated;
            IsKey = isKey;
            IsPrimaryKey = isPrimaryKey;
        }

        public ColumnMap PrimaryKey(bool isGenerated = false)
        {
            this.IsPrimaryKey = true;
            this.IsDatabaseGenerated = isGenerated;
            this.IsKey = true;
            return this;
        }

        //public void SetConversion(IConversion conversion)
        //{
        //    Conversion = conversion;
        //}

        public void HasConversion(IConversion conversion) => Conversion = conversion;
    }
}

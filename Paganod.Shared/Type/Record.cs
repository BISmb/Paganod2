using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Shared.Type
{
    public class Record : Dictionary<string, object>
    {
        public string Type { get; set; }
        public Guid Id => ContainsKey($"{Type}Id") ? this.Get<Guid>($"{Type}Id") : Guid.Empty;

        public Record(string strRecordType, Dictionary<string, object> dicData = default)
            : base(dicData)
        {
            Type = strRecordType;
        }

        public T Get<T>(string strKey)
        {
            return (T)this[strKey];
        }
    }
}

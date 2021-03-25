using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paganod.Shared.Type
{
    public interface IRecord
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public T Get<T>(string strKey);
        public void Set(string strKey, object value);
    }

    internal class RecordInternal : Dictionary<string, object> 
    {
        internal RecordInternal(Dictionary<string, object> data) : base(data) { }
    }

    public class Record : IRecord
    {
        private Dictionary<string, object> _internalDictionary = new();

        public Dictionary<string, object> Data => _internalDictionary;
        public string Type { get; set; }
        public Guid Id
        {
            get => ContainsKey($"{Type}Id") ? this.Get<Guid>($"{Type}Id") : Guid.Empty;
            set => Set($"{Type}Id", value);
        }

        public Record(string strRecordType, Dictionary<string, object> dicData = null)
        {
            Type = strRecordType;

            if (dicData != null)
                _internalDictionary = dicData;
        }

        public object this[string key]
        {
            get => Get(key);
            set => Set(key, value);
        }

        public object Get(string strKey)
        {
            return this._internalDictionary[strKey];
        }

        public T Get<T>(string strKey)
        {
            return (T)this._internalDictionary[strKey];
        }

        public void Set(string strKey, object value)
        {
            if (this.ContainsKey(strKey))
                this._internalDictionary[strKey] = value;
            else
                this._internalDictionary.Add(strKey, value);
        }

        public bool ContainsKey(string strKey)
        {
            return _internalDictionary.ContainsKey(strKey);
        }
    }
}

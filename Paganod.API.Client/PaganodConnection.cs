using Paganod.Shared.Type;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Paganod.API.Client
{
    public class PaganodConnection
    {
        private HttpClient _httpClient { get; }

        public PaganodConnection(string strConnectionString, TimeSpan tTimeout = default)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(strConnectionString);

            if (tTimeout == default) tTimeout = TimeSpan.FromSeconds(15);
            _httpClient.Timeout = tTimeout;
        }

        public IEnumerable<Record> Get(string strTableName)
        {
            return new List<Record>();
        }

        public IEnumerable<Record> Get<T>()
        {
            string strRecordType = typeof(T).Name;

            // Get Table Name from Record Type, if not found then throw exception

            string strTableName = "Transaction";

            return Get(strTableName);

            
        }
    }
}

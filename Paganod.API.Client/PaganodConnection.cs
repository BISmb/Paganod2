using Microsoft.EntityFrameworkCore;
using Paganod.Shared.Type;
using Paganod.Shared.Type.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
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

        public IQueryable<Record> GetAll(string strTableName)
        {
            return new List<Record>().AsQueryable();
        }

        public IQueryable<Record> Query(ODataQuery oQuery)
        {
            return Query<Record>(oQuery);
        }

        public IQueryable<T> Query<T>(ODataQuery oQuery)
            where T : Record
        {
            if (string.IsNullOrEmpty(oQuery.BaseTableName))
            {
                string strRecordType = typeof(T).Name;
                // Get Table Name from Record Type, if not found then throw exception
                oQuery.BaseTableName = "Transactions";
            }

            var results = Query<T>(oQuery);
            List<T> lstTypedResults = new();

            foreach (var result in results)
                lstTypedResults.Add((T)result);
                
            return results.AsQueryable<T>();
        }
    }
}

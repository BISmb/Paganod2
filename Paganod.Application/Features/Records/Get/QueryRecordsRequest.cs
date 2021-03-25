using AutoMapper;
using MediatR;
using Paganod.Data.Context;
using Paganod.Shared;
using Paganod.Shared.Type.Query;
using Paganod.Sql.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paganod.Application.Features.Records.Get
{
    public class QueryRecordsRequest : IRequest<Result<QueryRecordsResponse>>
    {
        public ODataQuery Query { get; set; }

        public QueryRecordsRequest(ODataQuery query)
        {
            Query = query;
        }
    }

    public class QueryRecordsRequestHandler : IRequestHandler<QueryRecordsRequest, Result<QueryRecordsResponse>>
    {
        private readonly IMapper _mapper;
        private readonly PaganodContext _dbContext;

        public QueryRecordsRequestHandler(PaganodContext dbContext/*, IMapper mapper*/)
        {
            _dbContext = dbContext;
            //_mapper = mapper;
        }

        public Task<Result<QueryRecordsResponse>> Handle(QueryRecordsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                QueryRecordsResponse oQueryResults = new QueryRecordsResponse();
                
                using(var db = _dbContext.GetNewDbConnection())
                {
                    oQueryResults.Records = db.GetRecords(request.Query);
                }

                //oQueryResults.Records = lstRecords;
                //oQueryResults.TotalItems =o 


                return Task.FromResult(Result<QueryRecordsResponse>.Success(oQueryResults));
            }
            catch (Exception)
            {
                throw;
            }


            throw new NotImplementedException();
        }
    }
}

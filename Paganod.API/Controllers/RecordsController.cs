using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using Paganod.Application.Features.Records.Get;
using Paganod.Data.Context;
using Paganod.Shared.Type;
using Paganod.Shared.Type.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paganod.API.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
    //[Route("Transactions")]
    [FromODataUri(Name = "transactions")]
    public class RecordsController : ODataController
    {
        private PaganodContext _dbContext;
        private IMediator _mediator;

        public RecordsController(PaganodContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;

            _dbContext.SchemaModels.Add(new Data.Entities.SchemaModel()); //Construct valid entity
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Run an OData Query of the form: /odata/transaction?$filter=TransactionId eq 5
        /// </summary>
        /// <returns></returns>
        [HttpGet("/odata/{TableDbName}", Order = 0)]
        public async Task<IActionResult> GetAsync([FromRoute] string TableDbName)
        {
            // get odata parameters
            Dictionary<string, string> oDataQueryParams = new();
            
            if(Request.Query.Count > 0)
                oDataQueryParams = Request.Query.Where(x => x.Key.StartsWith("$")).ToDictionary(x => x.Key, x => x.Value.ToString());

            ODataQuery query = new ODataQuery(TableDbName, oDataQueryParams);
            QueryRecordsRequest request = new QueryRecordsRequest(query);

            // create query handler and run
            try
            {
                var results = await _mediator.Send(request);
                return Ok(results);
            }
            catch (Exception)
            {
                return BadRequest();
                //throw;
            }
        }

        /// <summary>
        /// Get a record with @Id from @TableName
        /// </summary>
        /// <typeparam name="IdType">The type of the Id column</typeparam>
        /// <param name="TableName">The Table Name to retrieve the record from</param>
        /// <param name="Id">The Id of the record to retrieve</param>
        /// <returns>A record in json format</returns>
        [HttpGet("/{TableName}/{Id}", Name = nameof(GetRecordAsync), Order = 1)]
        public async Task<IActionResult> GetRecordAsync([FromRoute] string TableName, [FromRoute] Guid Id)
        {
            return Ok();
        }

        /// <summary>
        /// Gets many records from @TableName.
        /// </summary>
        /// <param name="TableName">The name of the table to get records from.</param>
        /// <param name="Page">The number page.</param>
        /// <param name="PageSize">The number of resukts per page. Max page size is 500 records.</param>
        /// <returns>An IEnumerable of Records</returns>
        //[HttpGet("/{TableName}/get")]
        //public async Task<IActionResult> GetRecords<IdType>([FromRoute] string TableName, [FromQuery] int Page, [FromQuery] int PageSize = 500) where IdType : struct
        //    => await ExecuteAsync(async (service) => await service.GetRecords(TableName, Page, PageSize));

        /// <summary>
        /// Create or Update, a record.
        /// </summary>
        /// <param name="Record"></param>
        /// <returns>The full record that was created or updated</returns>
        [HttpPost("/{TableName}", Name = nameof(SaveRecordAsync))]
        [HttpPatch("/{TableName}", Name = nameof(SaveRecordAsync))]
        public async Task<IActionResult> SaveRecordAsync([FromRoute] string TableName, [FromRoute] long RecordId)
        {
            return Ok();
        }

        /// <summary>
        /// Delete a record with @RecordId from @TableName
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="RecordId"></param>
        /// <returns>The number of affected records</returns>
        [HttpDelete("/{TableName}/{RecordId}", Name = nameof(DeleteRecordAsync))]
        [OpenApiOperation("Delete a record with @RecordId from @TableName")]
        public async Task<IActionResult> DeleteRecordAsync([FromRoute] string TableName, [FromRoute] Guid RecordId)
        {
            return Ok();
        }






        //    private static readonly string[] Summaries = new[]
        //    {
        //        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //    };

        //    private readonly ILogger<RecordsController> _logger;
        //    private PaganodContext _db;

        //    public RecordsController(ILogger<RecordsController> logger, PaganodContext context)
        //    {
        //        _logger = logger;

        //        _db = context;
        //        if (context.Forecasts.Count() == 0)
        //        {
        //            for (int i = 1; i < 5; i++)
        //            {
        //                context.Forecasts.Add(new WeatherForecast()
        //                {
        //                    Id = i,
        //                    Date = DateTime.Now.AddDays(i),
        //                    Summary = "stuff",
        //                });
        //            }

        //            context.SaveChanges();
        //        }
        //    }

        //public IActionResult Get()
        //{
        //    // get last element and use as the "TableName"
        //    string strTableName = Request.Path.Value.Substring(Request.Path.Value.LastIndexOf("/") + 1);

        //    // if parameters are in path (and they could contain "/" characters, this will only get the last element in the path
        //    if (Request.Path.Value.Contains("?"))
        //    {
        //        int firstParamIndex = Request.Path.Value.IndexOf("?");
        //        string pathNoParams = Request.Path.Value.Substring(0, Request.Path.Value.Length - firstParamIndex);
        //        strTableName = pathNoParams.Substring(pathNoParams.LastIndexOf("/"));
        //    }

        //    // This will put the query params in a dictionary, and the "$" will be passed to odata sql
        //    Dictionary<string, string> oDataQueryParams = Request.Query
        //                                                    .Where(x => x.Key.StartsWith("$"))
        //                                                    .ToDictionary(x => x.Key, x => x.Value.ToString());

        //    return Ok("Ok");
        //}

        //    [EnableQuery]
        //    public IActionResult Get(int key)
        //    {
        //        var rng = new Random();
        //        var rangee = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //        {
        //            Date = DateTime.Now.AddDays(index),
        //            TemperatureC = rng.Next(-20, 55),
        //            Summary = Summaries[rng.Next(Summaries.Length)]
        //        })
        //        .ToArray();
        //        Guid guid = Guid.NewGuid();
        //        return Ok(rangee.First(x => x.Id == key));
        //    }
        //}
    }
}

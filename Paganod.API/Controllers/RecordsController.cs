using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paganod.API.Controllers
{
    [OpenApiController("Records")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        //public RecordsController(RecordService recordService)
        //    : base(recordService) { }

        /// <summary>
        /// Get a record with @Id from @TableName
        /// </summary>
        /// <typeparam name="IdType">The type of the Id column</typeparam>
        /// <param name="TableName">The Table Name to retrieve the record from</param>
        /// <param name="Id">The Id of the record to retrieve</param>
        /// <returns>A record in json format</returns>
        [HttpGet("/{TableName}/{Id}", Name = nameof(GetRecordAsync))]
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
    }
}

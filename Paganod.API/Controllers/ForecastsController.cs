using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Paganod.Data.Database;
using Paganod.Shared.Type;
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
    public class ForecastsController : ODataController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ForecastsController> _logger;
        private PaganodContext _db;

        public ForecastsController(ILogger<ForecastsController> logger, PaganodContext context)
        {
            _logger = logger;

            _db = context;
            if (context.Forecasts.Count() == 0)
            {
                for (int i = 1; i < 5; i++)
                {
                    context.Forecasts.Add(new WeatherForecast()
                    {
                        Id = i,
                        Date = DateTime.Now.AddDays(i),
                        Summary = "stuff",
                    });
                }

                context.SaveChanges();
            }
        }

        //[HttpGet("/{TableName}")]
        [EnableQuery]
        public IEnumerable<WeatherForecast> Get()
        {
            var request = Request;

            Dictionary<string, string> oQueryParams = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            var rng = new Random();
            var rangee = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
            Guid guid = Guid.NewGuid();
            return Ok(rangee.First(x => x.Id == key));
        }
    }
}

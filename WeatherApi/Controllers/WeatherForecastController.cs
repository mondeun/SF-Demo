using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly Dictionary<string, List<WeatherReport>> Weather =
            new Dictionary<string, List<WeatherReport>>
            {
                {"Malmö", new List<WeatherReport> {new WeatherReport(DateTime.Now, 24)}}
            };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{location}")]
        public async Task<IEnumerable<WeatherReport>> Get(string location)
        {
            //var weatherStationActor =
            //    ActorProxy.Create<IWeatherStationActor>(new ActorId(location), new Uri("fabric:/SF-Demo/WeatherStationActorService"));

            //var results = await weatherStationActor.GetLocationWeatherTotal(null, CancellationToken.None);

            return Weather.GetValueOrDefault(location);
        }

        [HttpPost]
        public IActionResult PostWeather([FromBody] AddReport report)
        {
            if (Weather.ContainsKey(report.Location))
            {
                Weather.TryGetValue(report.Location, out var reports);
                reports?.Add(report.Report);
            }
            else
            {
                Weather.Add(report.Location, new List<WeatherReport> { report.Report });
            }

            return Ok();
        }
    }

    public class AddReport
    {
        public AddReport(string location, WeatherReport report)
        {
            Location = location;
            Report = report;
        }

        public string Location { get; }
        public WeatherReport Report { get; }
    }
}

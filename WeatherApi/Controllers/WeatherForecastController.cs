using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using WeatherStationActor.Interfaces;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/{location}")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IList<WeatherReport>> Get(string location)
        {
            _logger.LogInformation($"Fetching actor {location}'s state");
            var weatherStationActor =
                ActorProxy.Create<IWeatherStationActor>(new ActorId(location), new Uri("fabric:/SF_Demo/WeatherStationActorService"));

            return await weatherStationActor.GetLocationWeatherTotal(CancellationToken.None);
        }

        [HttpPost]
        public async Task<IList<WeatherReport>> PostWeather(string location, [FromBody] WeatherReport report)
        {
            _logger.LogInformation($"Updating actor {location}'s state", report);
            var weatherStationActor =
                ActorProxy.Create<IWeatherStationActor>(new ActorId(location), new Uri("fabric:/SF_Demo/WeatherStationActorService"));
            
            return await weatherStationActor.AddWeatherReport(report, CancellationToken.None);
        }
    }
}

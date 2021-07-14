using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using WeatherStationActor.Interfaces;

namespace WeatherStationActor
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class WeatherStationActor : Actor, IWeatherStationActor
    {
        //private IActorTimer _takeTemperatureTimer;

        /// <summary>
        /// Initializes a new instance of WeatherStationActor
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public WeatherStationActor(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override async Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            await StateManager.TryAddStateAsync("weather", new List<WeatherReport>());

            //_takeTemperatureTimer = RegisterTimer(TakeTemp, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30));
        }

        //protected override Task OnDeactivateAsync()
        //{
        //    if (_takeTemperatureTimer != null)
        //    {
        //        UnregisterTimer(_takeTemperatureTimer);
        //    }

        //    return base.OnDeactivateAsync();
        //}

        public async Task<IList<WeatherReport>> GetLocationWeatherTotal(CancellationToken cancellationToken)
        {
            return await StateManager.GetStateAsync<List<WeatherReport>>("weather", cancellationToken);
        }

        public async Task<IList<WeatherReport>> AddWeatherReport(WeatherReport report, CancellationToken cancellationToken)
        {
            return await StateManager.AddOrUpdateStateAsync("weather",
                new List<WeatherReport> { report }, (s, list) =>
                {
                    list.Add(report);
                    return list;
                },
                cancellationToken);
        }

        private async Task TakeTemp(object arg)
        {
            var rand = new Random();
            var report = new WeatherReport
            {
                Degrees = rand.Next(0, 35),
                Time = DateTime.Now
            };

            await StateManager.AddOrUpdateStateAsync("weather",
                new List<WeatherReport> { report }, (s, list) =>
                {
                    list.Add(report);
                    return list;
                });
        }
    }
}

using System;

namespace WeatherStationActor.Interfaces
{
    public class WeatherReport
    {
        public WeatherReport()
        {
        }

        public DateTime Time { get; set; }
        public int Degrees { get; set; }
    }
}

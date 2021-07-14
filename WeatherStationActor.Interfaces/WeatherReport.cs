using System;

namespace WeatherStationActor.Interfaces
{
    public class WeatherReport
    {
        public WeatherReport(DateTime time, int degrees)
        {
            Time = time;
            Degrees = degrees;
        }

        public DateTime Time { get; }
        public int Degrees { get; }
    }
}

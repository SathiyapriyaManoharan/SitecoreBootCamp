using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Bootcamp.Exercises.Promotions
{
    public class WeatherService
    {
        private string applicationId;

        public WeatherService(string applicationId)
        {
            this.applicationId = applicationId;
        }

        public async Task<Temperature> GetCurrentTemperature(string city, string country)
        {
            var client = new OpenWeatherMapClient(this.applicationId);

            var currentWeather = await client.CurrentWeather.GetByName(city, MetricSystem.Metric);

            return currentWeather.Temperature;
        }
    }
}

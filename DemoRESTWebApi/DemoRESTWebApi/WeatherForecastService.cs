using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoRESTWebApi
{
    public class WeatherForecastService : IWeatherForecastService
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(i => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(i),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
        }

        // Practical example
        public IEnumerable<WeatherForecast> GetExcercise(int resultsNumber, int minimalTemp, int maxTemp)
        {


            if (resultsNumber <= 0 || minimalTemp > maxTemp) { return null; }

            var rng = new Random();
            return Enumerable.Range(1, resultsNumber).Select(i => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(i),
                TemperatureC = rng.Next(minimalTemp, maxTemp),
                Summary = Summaries[rng.Next(Summaries.Length)]

            }).ToArray();
        }
    }
}

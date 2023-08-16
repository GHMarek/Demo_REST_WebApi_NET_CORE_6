namespace DemoRESTWebApi
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecast> Get();
        IEnumerable<WeatherForecast> GetExcercise(int resultsNumber, int minimalTemp, int maxTemp);
    }
}

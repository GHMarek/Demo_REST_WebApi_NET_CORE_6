using Microsoft.AspNetCore.Mvc;

namespace DemoRESTWebApi.Controllers
{
    [ApiController]
    // Route attribute indicates way of addressing method, ie. by controller name.
    // Sending request "get" and name of controller will access get method.
    // If many methods creates same endpoint, we get error.
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IWeatherForecastService _service;

        // Constructor uses IWeatherForecastService registered in service container.
        // Built in dependency container is able to create particular implementation of interface,
        // based on registration in Startup.cs/Program.cs.
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service, LibraryDataSeeder seeder)
        {
            _logger = logger;
            _service = service;
            seeder.Seed();
        }

        // By moving logic to WeatherForecastService, we achieve more readability and code separation.
        // http://localhost:5001/weatherforecast
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var result = _service.Get();

            return result;
        }

        // This method is accessible by url /WeatherForecast/ but we could add next Route [Route("anotherPathElement")]
        // To achieve more detailed route. We could also use [HttpGet("anotherPathElement")] for same effect.
        // We can also add arguments to get data to method. If we use [FromRoute], we have to modify route.
        // http://localhost:5001/weatherforecast/anotherPathElement/5
        // http://localhost:5001/weatherforecast/anotherPathElement/5?exampleArg=1
        [HttpGet("anotherPathElement/{maxResults}")]
        public IEnumerable<WeatherForecast> Get([FromQuery] int exampleArg, [FromRoute] int maxResults)
        {
            var result = _service.Get();

            return result;
        }

        // Post method accepts string from body (sent as JSON) and returns string as below.
        // ASP knows which method is called because of request http verb.
        // http://localhost:5001/weatherforecast
        [HttpPost]
        public string ExamplePost([FromBody] string exampleString)
        {
            return @$"Your example POST string: {exampleString}";
        }

        // This method returns different type, but also particular response status code.
        // http://localhost:5001/weatherforecast/anotherResultType/5
        [HttpPost("anotherResultType/{exampleInt}")]
        public ActionResult<string> ExamplePost([FromRoute] int exampleInt)
        {
            // Like this:
            //HttpContext.Response.StatusCode = 401;
            //return @$"Your example POST int: {exampleInt}";
            // or:
            //return StatusCode(401, @$"Your example POST int: {exampleInt.ToString()}");
            // or built in method:
            return NotFound(@$"Your example POST int: {exampleInt.ToString()}");
            // NotFound method is method from ControllerBase class, which is inherited by our controller.
        }

        // Practical example.
        [HttpGet("getexcercise")]
        //http://localhost:5001/weatherforecast/getexcercise?results=5&minTemp=5&maxTemp=10
        public IEnumerable<WeatherForecast> GetExcercise([FromQuery] int results, [FromQuery] int minTemp, [FromQuery] int maxTemp)
        {
            var result = _service.GetExcercise(results, minTemp, maxTemp);

            return result;
        }

        // Practical example.
        [HttpPost("generate/{results}")]
        //http://localhost:5001/weatherforecast/generate/1
        //{
        //    "MinTemp": 10,
        //    "MaxTemp": 30
        //}

        public ActionResult<string> PostExcercise([FromRoute] int results, [FromBody] WeatherInput inputObj)
        {
            bool status = true;

            if (results <= 0 || inputObj.MinTemp > inputObj.MaxTemp) { status = false; }

            IEnumerable<WeatherForecast> result = _service.GetExcercise(results, inputObj.MinTemp, inputObj.MaxTemp);

            return StatusCode(status ? 200 : 400, result);
        }

    }
}
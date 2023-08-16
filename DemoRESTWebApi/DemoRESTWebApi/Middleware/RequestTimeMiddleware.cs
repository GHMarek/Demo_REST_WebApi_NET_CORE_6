using DemoRESTWebApi.Exceptions;
using System.Diagnostics;

namespace DemoRESTWebApi.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger) 
        { 
            _logger = logger;
            _stopwatch = new Stopwatch();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            await next.Invoke(context);
            _stopwatch.Stop();

            var milSecs = _stopwatch.ElapsedMilliseconds;

            if (milSecs > 4000) 
            {
                var message = $"{context.Request.Method} at {context.Request.Path} took {milSecs} ms";
                _logger.LogTrace(message);
            }
        }
    }
}

using Helper;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics;

namespace ApplicantService.Helper
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                string logMessage = $"Request {context.Request.Method} {context.Request.Path} executed in {elapsedMilliseconds} ms";

                // Log the message using your custom logging method
                //ExceptionLogging.LogException(logMessage);
            

             
            }
        }
    }
}

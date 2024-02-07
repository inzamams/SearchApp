using Microsoft.AspNetCore.Http.Extensions;
using ILogger = Serilog.ILogger;

namespace SearchApp.Extensions
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next,
        ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = UriHelper.GetDisplayUrl(context.Request);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            _logger.Information($"REQUEST METHOD: {context.Request.Method}, REQUEST BODY: {requestBodyText}, REQUEST URL: {url}");

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            await _next(context);
            context.Request.Body = originalRequestBody;
        }
    }
}

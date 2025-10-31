using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Gateway.Middlewares;

public class LoggingAndPerformanceMiddleware(RequestDelegate next, ILogger<LoggingAndPerformanceMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.ContainsKey("X-Request-ID")
            ? context.Request.Headers["X-Request-ID"].ToString()
            : Guid.NewGuid().ToString();

        context.Response.Headers.Append("X-Request-ID", correlationId);

        var sw = Stopwatch.StartNew();
        logger.LogInformation("[{CorrelationId}] Начало {Method} {Path}", correlationId, context.Request.Method, context.Request.Path);

        try
        {
            await next(context);
        }
        finally
        {
            sw.Stop();
            var elapsedMs = sw.ElapsedMilliseconds;

            if (elapsedMs > 500) 
            {
                logger.LogWarning("[{CorrelationId}] Слишком медленный запрос: {Method} {Path}  {Elapsed} ms",
                    correlationId, context.Request.Method, context.Request.Path, elapsedMs);
            }
            else
            {
                logger.LogInformation("[{CorrelationId}] Завершено {Method} {Path} {Elapsed} ms",
                    correlationId, context.Request.Method, context.Request.Path, elapsedMs);
            }
        }
    }
}
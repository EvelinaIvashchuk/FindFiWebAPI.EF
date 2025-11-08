using Serilog.Context;

namespace FindFi.Ef.Api.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        const string headerName = "X-Correlation-ID";
        var correlationId = context.Request.Headers.TryGetValue(headerName, out var values) && !string.IsNullOrWhiteSpace(values)
            ? values.ToString()
            : Guid.NewGuid().ToString("N");

        context.Response.Headers[headerName] = correlationId;
        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("RequestPath", context.Request.Path.ToString()))
        {
            await next(context);
        }
    }
}

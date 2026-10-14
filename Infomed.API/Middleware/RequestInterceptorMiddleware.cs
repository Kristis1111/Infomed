namespace Infomed.API.Middleware;

public class RequestInterceptorMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var values)
            && Guid.TryParse(values.FirstOrDefault(), out var parsedId)
                ? parsedId.ToString("D")
                : Guid.NewGuid().ToString("D");

        context.TraceIdentifier = correlationId;
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        return next(context);
    }
}

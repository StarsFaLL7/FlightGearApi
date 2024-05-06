using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using WebApi.Controllers.Base;

namespace webapi.Middlewares;

public class FlightGearExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusResponse = new BasicStatusResponse
        {
            Status = BasicStatusEnum.Failed.ToString(),
            Comment = exception.ToString()
        };
        if (exception.InnerException != null)
        {
            statusResponse.Comment += $"; Inner Exception: {exception.InnerException.Message}";
        }

        statusResponse.Comment += $";  Source: {exception.Source}";
        Console.WriteLine($"Exception occured: {exception}");
        httpContext.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(statusResponse);
        await httpContext.Response.WriteAsync(json, cancellationToken);
        return true;
    }
}
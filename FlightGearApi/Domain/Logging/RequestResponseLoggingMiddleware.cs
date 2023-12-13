using System.Text;

namespace FlightGearApi.Domain.Logging;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {

        var requestBody = await ReadRequestBody(context);
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            await StaticLogger.LogAsync(LogLevel.Information, $"Request: {context.Request.Method} {context.Request.Path}");
        }
        else
        {
            await StaticLogger.LogAsync(LogLevel.Information, $"Request: {context.Request.Method} {context.Request.Path}, Body: {requestBody}");
        }
        
        using (var newResponseBody = new MemoryStream())
        {
            var originalResponseBody = context.Response.Body;
            context.Response.Body = newResponseBody;

            // Вызов следующего элемента конвейера
            await _next(context);

            // Восстановление оригинального потока ответа
            context.Response.Body = originalResponseBody;

            // Чтение тела ответа
            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(newResponseBody).ReadToEndAsync();

            // Логирование тела ответа
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                await StaticLogger.LogAsync(LogLevel.Information, 
                    $"Finished handling request {context.Request.Method} {context.Request.Path}, got response code: {context.Response.StatusCode}");
            }
            else
            {
                await StaticLogger.LogAsync(LogLevel.Information, 
                    $"Finished handling request {context.Request.Method} {context.Request.Path}, got response code: {context.Response.StatusCode}, Body: {responseBody}");
            }

            // Запись тела ответа в оригинальный поток
            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
        }
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        var requestBody = string.Empty;

        if (context.Request.Body.CanRead)
        {
            using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            
            context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
        }
        
        return requestBody;
    }
    
    private async Task<string> ReadResponseBody(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            memoryStream.Seek(0, SeekOrigin.Begin);
            
            var responseBody = new StreamReader(memoryStream).ReadToEnd();
            
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);

            return responseBody;
        }
    }
}
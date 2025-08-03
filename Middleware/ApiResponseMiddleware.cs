using System.Net;
using System.Text.Json;

namespace ProjectManagementAPI.Middleware
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    // Catch the original response
        //    var originalBody = context.Response.Body;

        //    using var memStream = new MemoryStream();
        //    context.Response.Body = memStream;

        //    await _next(context); // Continue pipeline

        //    // Reset stream position
        //    memStream.Seek(0, SeekOrigin.Begin);
        //    var responseBody = new StreamReader(memStream).ReadToEnd();

        //    // Reset stream position again
        //    memStream.Seek(0, SeekOrigin.Begin);

        //    context.Response.Body = originalBody;

        //    // Skip if already JSON formatted
        //    if (context.Response.ContentType != null &&
        //        context.Response.ContentType.Contains("application/json"))
        //    {
        //        var wrapped = new
        //        {
        //            status = context.Response.StatusCode == (int)HttpStatusCode.OK,
        //            message = context.Response.StatusCode == (int)HttpStatusCode.OK ? "Success" : "Error",
        //            data = JsonSerializer.Deserialize<object>(responseBody)
        //        };

        //        var wrappedJson = JsonSerializer.Serialize(wrapped);

        //        context.Response.ContentType = "application/json";
        //        await context.Response.WriteAsync(wrappedJson);
        //    }
        //    else
        //    {
        //        // For non-JSON content, just write original
        //        await context.Response.WriteAsync(responseBody);
        //    }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            memoryStream.Seek(0, SeekOrigin.Begin);

            context.Response.Body = originalBodyStream;

            // Handle only JSON responses
            if (context.Response.ContentType != null &&
                context.Response.ContentType.Contains("application/json"))
            {
                var statusCode = context.Response.StatusCode;
                var isSuccess = statusCode >= 200 && statusCode < 300;

                var response = new
                {
                    status = isSuccess,
                    message = isSuccess ? "Success" : "Error",
                    data = string.IsNullOrWhiteSpace(responseBody) ? null : JsonSerializer.Deserialize<object>(responseBody)
                };

                var responseJson = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(responseJson);
            }
            else
            {
                // Other content types
                await context.Response.WriteAsync(responseBody);
            }
        
    }
    }
}

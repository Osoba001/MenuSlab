using System.Net;
using DeviceDetectorNET;

namespace MenuSlab.API
{
    public class GlobalMiddleware(ILogger<GlobalMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<GlobalMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    await next(context);
                    return;
                }

                //var userAgent = context.Request.Headers["User-Agent"].ToString();

                //if (!string.IsNullOrEmpty(userAgent))
                //{
                //    var deviceDetector = new DeviceDetector(userAgent);
                //    deviceDetector.Parse();

                //    var deviceType = deviceDetector.GetDeviceName();

                //    if (deviceType != "smartphone" && deviceType != "tablet")
                //    {
                //        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //        await context.Response.WriteAsync("Access restricted on this device.");
                //        return;
                //    }
                //}
                //else
                //{
                //    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                //    await context.Response.WriteAsync("Access restricted on this device.");
                //    return;
                //}

                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in GlobalExceptionHandlerMiddleware");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(new ErrorDetail
                {
                    Title="Server Error",
                    StatusCode = context.Response.StatusCode,
                    Message = "Internal Server Error. Please try again later."
                }.ToString());
            }
        }
    }

    public class ErrorDetail
    {
        public required string Title { get; set; }
        public required int StatusCode { get; set; }
        public required string Message { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}, \n Error code: {StatusCode}, \nMessage: {Message}";
        }
    }
}

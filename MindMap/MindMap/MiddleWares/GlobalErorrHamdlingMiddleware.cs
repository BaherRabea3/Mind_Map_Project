using System.Net;
using System.Text.Json;

namespace MindMapManager.WebAPI.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex.Message switch
            {
                var m when m.Contains("not found") => HttpStatusCode.NotFound,
                var m when m.Contains("forbidden") => HttpStatusCode.Forbidden,
                var m when m.Contains("already") => HttpStatusCode.BadRequest,
                var m when m.Contains("Failed") => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                error = ex.Message,
                statusCode = (int)statusCode
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

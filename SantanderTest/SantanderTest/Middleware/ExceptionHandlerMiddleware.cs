using System.Net;
using System.Text.Json;

namespace SantanderTest.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //TODO Implement wrapper for the base Exception to provide additional expection information
            _logger.LogError(ex, "An unexpected error occurred.");

            var result = JsonSerializer.Serialize(new
            {
                message = "An error occurred while processing your request.",
                detail = ex.Message // Consider exposing less detail in production
            });

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(result);
        }
    }
}


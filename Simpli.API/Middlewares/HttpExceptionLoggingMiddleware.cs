using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;

namespace Simpli.API.Middlewares
{
    public static class HttpExceptionLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseHttpExceptionLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpExceptionLoggingMiddleware>();
        }
    }

    public class HttpExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpExceptionLoggingMiddleware> _logger;
        private const string JsonContentType = "application/json";
        const string InternalErrorMsg = "There's an error while executing your request, please try again.";

        public HttpExceptionLoggingMiddleware(RequestDelegate next, ILogger<HttpExceptionLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline

                await _next(context);
            }
            catch (Exception ex)
            {
                await WriteResponseAsync(context, ex);

                _logger.LogError(InternalErrorMsg, ex);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var responseData = JsonConvert.SerializeObject(new
            {
                message = ex.Message
            });
            await context.Response.WriteAsync(responseData);

        }
    }
}

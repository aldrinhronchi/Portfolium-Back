using System.Text.Json;
using Portfolium_Back.Extensions.Helpers;
using Portfolium_Back.Models.Entities;

namespace Portfolium_Back.Extensions.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly ILogger<ErrorMiddleware> _logger;
        private readonly ErrorHandlerService _errorHandlerService;

        public ErrorMiddleware(RequestDelegate requestDelegate, ILogger<ErrorMiddleware> logger, ErrorHandlerService errorHandlerService)
        {
            this.requestDelegate = requestDelegate;
            this._logger = logger;
            this._errorHandlerService = errorHandlerService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await requestDelegate(context);
            }
            catch (Exception exception)
            {
                ErrorInfo errorInfo = await _errorHandlerService.FormatExceptionAsync(exception);
                context.Response.StatusCode = errorInfo.StatusCode;
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = null
                };

                await context.Response.WriteAsJsonAsync(errorInfo, options);
            }
        }
    }
} 
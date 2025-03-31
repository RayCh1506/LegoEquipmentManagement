using EquipmentManagementPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EquipmentManagementPlatform.API.Middleware
{
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
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
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError("An {exceptionName} occured, {ex}", ex.GetType().Name, ex);

            ProblemDetails problemDetails = new ProblemDetails();
            if (ex is ArgumentException || ex is InvalidEquipmentStateTransitionException || ex is EquipmentStartException || ex is EquipmentStopException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
            }
            else if(ex is EquipmentNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;                
                problemDetails.Status = (int)HttpStatusCode.NotFound;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Status = (int)HttpStatusCode.InternalServerError;
            }

            problemDetails.Detail = ex.Message;
            problemDetails.Title = ex.GetType().Name;
            problemDetails.Type = ex.GetType().Name;
            context.Response.ContentType = "application/json";
            var problemDetailsJson = JsonSerializer.Serialize(problemDetails);

            await context.Response.WriteAsync(problemDetailsJson);

        }
    }
}

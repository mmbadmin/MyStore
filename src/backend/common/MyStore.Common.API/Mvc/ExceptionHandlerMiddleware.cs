namespace MyStore.Common.API.Mvc
{
    using MyStore.Common.API.Models;
    using MyStore.Common.Application.Exceptions;
    using MyStore.Common.Utilities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception was thrown by the application");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new MyStoreResponse();
            var code = HttpStatusCode.InternalServerError;
            switch (exception)
            {
                case ValidationException ex:
                {
                    code = HttpStatusCode.BadRequest;
                    response.Message = ex.Messages;
                    break;
                }
                case BaseException ex:
                {
                    code = (HttpStatusCode)ex.Status;
                    response.Message = ex.GetMessages();
                    break;
                }
                default:
                {
                    response.Message = exception.GetMessages();
                    break;
                }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(JsonHelper.ToJson(response));
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mushka.Core.Extensibility.Logging;
using Mushka.WebApi.ClientModels;

namespace Mushka.WebApi.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const int DefaultExceptionStatusCode = (int)HttpStatusCode.InternalServerError;
        private const string DefaultErrorMessage = "Unhandled error occurred.";

        private readonly ILogger logger;

        public ApiExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            HandleException(context);
            return base.OnExceptionAsync(context);
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Exception exception = context.Exception;

            logger.LogError(exception);
#if DEBUG
            ApiExceptionResponse response = new ApiExceptionResponse(DefaultErrorMessage, exception.StackTrace);
#else
            ApiExceptionResponse response = new ApiExceptionResponse(DefaultErrorMessage);
#endif
            context.Result = new JsonResult(response) { StatusCode = DefaultExceptionStatusCode };
        }
    }
}
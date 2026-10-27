using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Mushka.Api.ClientModels;
using Mushka.Application.Exceptions;

namespace Mushka.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private const int DefaultExceptionStatusCode = (int)HttpStatusCode.InternalServerError;
    private const string DefaultErrorMessage = "Unhandled error occurred.";

    private readonly ILogger<ApiExceptionFilterAttribute> _logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        _logger = logger;
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
        if (context.Exception is NotFoundException notFound)
        {
            context.Result = Error(HttpStatusCode.NotFound, notFound.ErrorKey);
            return;
        }

        if (context.Exception is BusinessRuleException businessRule)
        {
            context.Result = Error(HttpStatusCode.BadRequest, businessRule.ErrorKey);
            return;
        }

        _logger.LogError(context.Exception, DefaultErrorMessage);
#if DEBUG
        var response = new ApiExceptionResponse(DefaultErrorMessage, context.Exception.StackTrace);
#else
        var response = new ApiExceptionResponse(DefaultErrorMessage);
#endif
        context.Result = new JsonResult(response) { StatusCode = DefaultExceptionStatusCode };
    }

    private static JsonResult Error(HttpStatusCode statusCode, string errorKey) =>
        new JsonResult(new ErrorResponseModel { Errors = new[] { errorKey } })
        {
            StatusCode = (int)statusCode
        };
}

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mushka.Api.Filters;

public class ModelStateValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var errors = context.ModelState
            .SelectMany(entry => entry.Value.Errors)
            .Select(error => string.IsNullOrEmpty(error.ErrorMessage) ? error.Exception?.Message : error.ErrorMessage)
            .Where(message => !string.IsNullOrEmpty(message))
            .ToArray();

        context.Result = new BadRequestObjectResult(new { errors });
    }
}

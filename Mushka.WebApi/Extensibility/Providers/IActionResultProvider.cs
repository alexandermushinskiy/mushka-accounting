using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Validation;

namespace Mushka.WebApi.Extensibility.Providers
{
    public interface IActionResultProvider
    {
        IActionResult Get(object responseModel, int successfulStatusCode = StatusCodes.Status200OK);

        IActionResult GetFailedResult(IValidationResponse validationResponse);
    }
}
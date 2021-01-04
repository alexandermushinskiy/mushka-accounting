using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;

namespace Mushka.WebApi.Extensibility.Providers
{
    public interface IActionResultProvider
    {
        IActionResult GetNew(OperationResult operationResult, object responseModel);

        IActionResult Get(object responseModel, int successfulStatusCode = StatusCodes.Status200OK);
        
        IActionResult Get(object responseModel, ValidationStatusType statusType);
    }
}
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Validation;

namespace Mushka.WebApi.Extensibility.Providers
{
    public interface IActionResultProvider
    {
        IActionResult GetNew(OperationResult operationResult, object responseModel);
    }
}
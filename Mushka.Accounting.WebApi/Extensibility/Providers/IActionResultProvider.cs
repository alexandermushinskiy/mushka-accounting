using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.WebApi.ClientModels;

namespace Mushka.Accounting.WebApi.Extensibility.Providers
{
    public interface IActionResultProvider
    {
        IActionResult Get(
            ResourceResponseModelBase resourceResponseModel,
            int successfulStatusCode = StatusCodes.Status200OK);
    }
}
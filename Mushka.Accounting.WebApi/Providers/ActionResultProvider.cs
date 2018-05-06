using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Providers
{
    public class ActionResultProvider : IActionResultProvider
    {
        private const int DefaultSuccessfulStatusCode = StatusCodes.Status200OK;

        public IActionResult Get(ResourceResponseModelBase resourceResponseModel, int successfulStatusCode = StatusCodes.Status200OK) =>
            new ObjectResult(resourceResponseModel)
            {
                StatusCode = Get(resourceResponseModel.Messages.Select(m => m.StatusCode).Cast<int>(), successfulStatusCode)
            };

        private static int Get(IEnumerable<int> statusCodes, int successfulStatusCode = DefaultSuccessfulStatusCode)
        {
            int[] distinctStatusCodes = statusCodes
                .Distinct().ToArray();

            switch (distinctStatusCodes.Length)
            {
                case 0:
                    return successfulStatusCode;
                case 1:
                    return distinctStatusCodes[0] == DefaultSuccessfulStatusCode ? successfulStatusCode : distinctStatusCodes[0];
            }

            return StatusCodes.Status207MultiStatus;
        }
    }
}
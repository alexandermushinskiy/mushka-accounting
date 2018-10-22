using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Validation.Enums;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Extensibility.Providers;

namespace Mushka.Accounting.WebApi.Providers
{
    internal class ActionResultProvider : IActionResultProvider
    {
        private readonly IMapper mapper;

        public ActionResultProvider(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public IActionResult Get(object responseModel, int successfulStatusCode = StatusCodes.Status200OK)
        {
            return new ObjectResult(responseModel)
            {
                StatusCode = (responseModel as ResponseModelBase)?.StatusCode ?? successfulStatusCode
            };
        }

        public IActionResult GetFailedResult(IValidationResponse validationResponse)
        {
            var statusCode = mapper.Map<ValidationStatusType, int?>(validationResponse.ValidationResult.Status);
            var responseModel = mapper.Map<IValidationResult, ResponseModelBase>(validationResponse.ValidationResult);

            return Get(responseModel, statusCode.Value);
        }
    }
}
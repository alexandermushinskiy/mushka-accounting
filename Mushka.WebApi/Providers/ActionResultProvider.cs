using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Providers
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
                StatusCode = (responseModel as ResponseModelBaseOld)?.StatusCode ?? successfulStatusCode
            };
        }

        public IActionResult GetFailedResult(IValidationResponse validationResponse)
        {
            var statusCode = mapper.Map<ValidationStatusType, int?>(validationResponse.ValidationResult.Status);
            var responseModel = mapper.Map<IValidationResult, ResponseModelBaseOld>(validationResponse.ValidationResult);

            return Get(responseModel, statusCode.Value);
        }

        public IActionResult Get(object responseModel, ValidationStatusType statusType)
        {
            return new ObjectResult(responseModel)
            {
                StatusCode = mapper.Map<ValidationStatusType, int?>(statusType)
            };
        }
    }
 }
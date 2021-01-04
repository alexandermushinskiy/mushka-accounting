using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Validation;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Order;
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

        // TODO: remove other methods
        public IActionResult GetNew(OperationResult operationResult, object responseModel)
        {
            var statusCode = mapper.Map<ValidationStatusType, int?>(operationResult.Status);

            if (operationResult.IsSuccess)
            {
                return new ObjectResult(responseModel) { StatusCode = statusCode };
            }

            var errorResponseModel = mapper.Map<IEnumerable<FieldError>, ErrorResponseModel>(operationResult.Errors);
            return new ObjectResult(errorResponseModel) { StatusCode = statusCode };
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
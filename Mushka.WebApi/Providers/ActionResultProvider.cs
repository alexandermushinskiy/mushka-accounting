using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Validation;
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
    }
 }
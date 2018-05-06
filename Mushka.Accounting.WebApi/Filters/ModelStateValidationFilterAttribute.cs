using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.WebApi.ClientModels;

namespace Mushka.Accounting.WebApi.Filters
{
    public class ModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly IMapper mapper;

        public ModelStateValidationFilterAttribute(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                IEnumerable<IValidationResult> errors = context.ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(modelError => ValidationResult.CreateWarning(GetMessage(modelError))).ToArray();

                context.Result = new BadRequestObjectResult(mapper.Map<IEnumerable<IValidationResult>, IEnumerable<MessageResponseModel>>(errors));
            }
        }

        private static string GetMessage(ModelError modelError) =>
            !String.IsNullOrEmpty(modelError.ErrorMessage)
                ? modelError.ErrorMessage
                : modelError.Exception?.Message;
    }
}
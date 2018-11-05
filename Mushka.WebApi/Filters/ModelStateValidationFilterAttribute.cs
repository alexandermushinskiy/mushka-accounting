using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mushka.WebApi.ClientModels;

namespace Mushka.WebApi.Filters
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
                IEnumerable<string> errorMessages = context.ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(GetMessage)
                    .ToArray();

                context.Result = new BadRequestObjectResult(mapper.Map<IEnumerable<string>, ResponseModelBase>(errorMessages));
            }
        }

        private static string GetMessage(ModelError modelError) =>
            !String.IsNullOrEmpty(modelError.ErrorMessage)
                ? modelError.ErrorMessage
                : modelError.Exception?.Message;
    }
}
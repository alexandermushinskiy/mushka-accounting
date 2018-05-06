using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mushka.Accounting.Core.Validation.Enums;

namespace Mushka.Accounting.WebApi.Resolvers
{
    public class StatusCodeTypeConverter : ITypeConverter<ValidationStatusType, int?>
    {
        public int? Convert(ValidationStatusType source, int? destination, ResolutionContext context)
        {
            switch (source)
            {
                case ValidationStatusType.Success:
                    return StatusCodes.Status200OK;
                case ValidationStatusType.BadOperation:
                    return StatusCodes.Status400BadRequest;
                case ValidationStatusType.NotFound:
                    return StatusCodes.Status404NotFound;
                case ValidationStatusType.Error:
                    return StatusCodes.Status500InternalServerError;
                case ValidationStatusType.Forbidden:
                    return StatusCodes.Status403Forbidden;
                default:
                    throw new NotImplementedException($"{source} doesn't have registered status code.");
            }
        }
    }
}
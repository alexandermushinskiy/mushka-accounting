using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.WebApi.ClientModels.Order;

namespace Mushka.WebApi.Resolvers
{
    public class ErrorResponseConverter : ITypeConverter<IEnumerable<FieldError>, ErrorResponseModel>
    {
        public ErrorResponseModel Convert(IEnumerable<FieldError> source, ErrorResponseModel destination, ResolutionContext context)
        {
            return new ErrorResponseModel
            {
                Errors = source.Select(x => x.ErrorKey)
            };
        }
    }
}
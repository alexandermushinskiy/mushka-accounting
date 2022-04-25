using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<ValidationStatusType, int?>().ConvertUsing<StatusCodeTypeConverter>();

            CreateMap<IEnumerable<FieldError>, ErrorResponseModel>().ConvertUsing<ErrorResponseConverter>();
            CreateMap<IEnumerable<string>, ErrorResponseModel>().ConvertUsing<ErrorResponseConverter>();
        }
    }
}
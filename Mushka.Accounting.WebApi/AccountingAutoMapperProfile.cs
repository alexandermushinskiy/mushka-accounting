using System.Collections.Generic;
using AutoMapper;
using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Core.Validation.Enums;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.Resolvers;

namespace Mushka.Accounting.WebApi
{
    public class AccountingAutoMapperProfile : Profile
    {
        public AccountingAutoMapperProfile()
        {
            CreateMap<ValidationStatusType, int?>().ConvertUsing<StatusCodeTypeConverter>();

            CreateMap<IValidationResult, MessageResponseModel>()
                .ForMember(dest => dest.LevelType, opts => opts.MapFrom(src => src.Level))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Message, opts => opts.MapFrom(src => src.Message));

            CreateMap<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));

            CreateMap<ValidationResponse<Category>, CategoryResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));

            CreateMap<CategoryRequestModel, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
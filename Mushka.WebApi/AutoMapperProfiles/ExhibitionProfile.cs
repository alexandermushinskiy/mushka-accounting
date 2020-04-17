using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class ExhibitionProfile : Profile
    {
        public ExhibitionProfile()
        {
            CreateMap<ExhibitionRequestModel, Exhibition>().ConvertUsing<ExhibitionRequestConverter>();
            CreateMap<Exhibition, ExhibitionModel>().ConvertUsing<ExhibitionConverter>();
            CreateMap<ExhibitionProduct, ExhibitionProductModel>().ConvertUsing<ExhibitionConverter>();

            CreateMap<ValidationResponse<Exhibition>, ExhibitionResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Exhibition>>, ExhibitionsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Exhibition>>, ExhibitionsListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionsListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<ExhibitionProduct>>, ExhibitionProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionProductResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
        }
    }
}
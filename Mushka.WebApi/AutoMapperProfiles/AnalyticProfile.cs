using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.WebApi.ClientModels.Analytics;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class AnalyticProfile : Profile
    {
        public AnalyticProfile()
        {
            CreateMap<ValidationResponse<Balance>, BalanceResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<PopularProduct>>, PopularProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<PopularCity>>, PopularCitiesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<OrdersCount>>, OrdersCountResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<SoldProductsCount>>, SoldProductsCountResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
        }
    }
}
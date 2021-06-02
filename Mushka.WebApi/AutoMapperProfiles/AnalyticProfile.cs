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
            CreateMap<OperationResult<Balance>, AnalyticsResponseModel<Balance>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<OperationResult<IEnumerable<PopularProduct>>, AnalyticsResponseModel<PopularProduct[]>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<OperationResult<IEnumerable<PopularCity>>, AnalyticsResponseModel<PopularCity[]>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<OperationResult<IEnumerable<OrdersCount>>, AnalyticsResponseModel<OrdersCount[]>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));

            CreateMap<OperationResult<IEnumerable<SoldProductsCount>>, AnalyticsResponseModel<SoldProductsCount[]>>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
        }
    }
}
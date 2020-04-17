using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class CorporateOrderProfile : Profile
    {
        public CorporateOrderProfile()
        {
            CreateMap<CorporateOrderRequestModel, CorporateOrder>().ConvertUsing<CorporateOrderRequestConverter>();
            CreateMap<CorporateOrder, CorporateOrderModel>().ConvertUsing<CorporateOrderConverter>();

            CreateMap<ValidationResponse<CorporateOrder>, CorporateOrderResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CorporateOrderResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<CorporateOrder>>, CorporateOrdersListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CorporateOrdersListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
        }
    }
}
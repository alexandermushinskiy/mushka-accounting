using System.Collections.Generic;
using System.Linq;
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

            CreateMap<OperationResult<CorporateOrder>, CorporateOrderResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CorporateOrderResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<IEnumerable<CorporateOrder>>, CorporateOrdersListResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CorporateOrdersListResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}
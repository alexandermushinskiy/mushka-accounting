using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Order;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderRequestModel, Order>().ConvertUsing<OrderRequestConverter>();
            CreateMap<Order, OrderModel>().ConvertUsing<OrderConverter>();
            CreateMap<OrderProduct, OrderProductModel>().ConvertUsing<OrderConverter>();

            CreateMap<OperationResult<Order>, OrderResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<IEnumerable<Order>>, OrdersListResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrdersListResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<IEnumerable<OrderProduct>>, OrderProductsResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderProductResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}
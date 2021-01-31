using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.WebApi.ClientModels.Order;
using Mushka.WebApi.ClientModels.Order.GetById;
using Mushka.WebApi.ClientModels.Order.GetDefaultProducts;
using Mushka.WebApi.ClientModels.Order.Search;
using Mushka.WebApi.ClientModels.Order.ValidateOrderNumber;
using Mushka.WebApi.Resolvers.Orders;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderRequestModel, Order>().ConvertUsing<OrderRequestConverter>();
            CreateMap<Order, OrderModel>().ConvertUsing<OrderConverter>();
            CreateMap<OrderProduct, OrderProductModel>().ConvertUsing<OrderConverter>();
            CreateMap<SearchOrdersRequestModel, SearchOrdersFilter>().ConvertUsing<OrdersFilterRequestConverter>();

            CreateMap<OrderSummaryDto, OrderSummaryModel>();
            CreateMap<OperationResult<ItemsList<OrderSummaryDto>>, SearchOrdersResponseModel>()
                .ConvertUsing<SearchOrdersResponseConverter>();

            CreateMap<OperationResult<IEnumerable<OrderProduct>>, OrderDefaultProductResponseModel>()
                .ConvertUsing<OrderDefaultProductsResponseConverter>();

            CreateMap<OperationResult<Order>, OrderResponseModel>()
                .ConvertUsing<OrderResponseConverter>();

            CreateMap<OperationResult<bool>, ValidateOrderNumberResponseModel>()
                .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.Data));
        }
    }
}
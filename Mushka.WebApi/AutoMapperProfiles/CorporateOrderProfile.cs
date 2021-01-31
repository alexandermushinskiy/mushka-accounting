using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.CorporateOrder;
using Mushka.WebApi.ClientModels.CorporateOrder.GetById;
using Mushka.WebApi.ClientModels.CorporateOrder.GetAll;
using Mushka.WebApi.ClientModels.CorporateOrder.ValidateOrderNumber;
using Mushka.WebApi.Resolvers.CorporateOrders;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class CorporateOrderProfile : Profile
    {
        public CorporateOrderProfile()
        {
            CreateMap<CorporateOrderRequestModel, CorporateOrder>().ConvertUsing<CorporateOrderRequestConverter>();

            CreateMap<CorporateOrder, CorporateOrderSummaryModel>().ConvertUsing<CorporateOrderSummaryConverter>();
            CreateMap<OperationResult<IEnumerable<CorporateOrder>>, GetAllResponseModel>()
                .ConvertUsing<GetAllResponseResolver>();

            CreateMap<CorporateOrder, CorporateOrderModel>().ConvertUsing<CorporateOrderConverter>();
            CreateMap<CorporateOrderProduct, CorporateOrderProductModel>().ConvertUsing<CorporateOrderProductConverter>();
            CreateMap<OperationResult<CorporateOrder>, CorporateOrderResponseModel>()
                .ConvertUsing<CorporateOrderResponseConverter>();

            CreateMap<OperationResult<bool>, ValidateCorporateOrderNumberResponseModel>()
                .ForMember(dest => dest.IsValid, opt => opt.MapFrom(src => src.Data));
        }
    }
}
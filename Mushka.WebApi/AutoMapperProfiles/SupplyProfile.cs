using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Dto;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Supply;
using Mushka.WebApi.ClientModels.Supply.Describe;
using Mushka.WebApi.ClientModels.Supply.Search;
using Mushka.WebApi.Resolvers;
using Mushka.WebApi.Resolvers.Suppliers;
using Mushka.WebApi.Resolvers.Supplies;
using SupplyModel = Mushka.WebApi.ClientModels.Supply.SupplyModel;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class SupplyProfile : Profile
    {
        public SupplyProfile()
        {
            CreateMap<OperationResult<ItemsWithTotalCount<Supply>>, SearchSuppliesResponseModel>()
                .ConvertUsing<SearchSuppliesResponseConverter>();

            CreateMap<OperationResult<Supply>, DescribeSupplyResponseModel>()
                .ConvertUsing<DescribeSupplyResponseConverter>();

            CreateMap<SupplyRequestModel, Supply>().ConvertUsing<SupplyRequestConverter>();
            CreateMap<Supply, SupplyModel>().ConvertUsing<SupplyConverter>();

            CreateMap<SuppliesFiltersRequestModel, SuppliesFiltersModel>();

            //CreateMap<OperationResult<Supply>, SupplyResponseModel>()
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplyResponseResolver>())
            //    .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
            //    .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            //CreateMap<OperationResult<ItemsWithTotalCount<Supply>>, DataWithTotalCountResponseModel<SupplyListModel>>()
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SuppliesListResponseResolver>())
            //    .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
            //    .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}
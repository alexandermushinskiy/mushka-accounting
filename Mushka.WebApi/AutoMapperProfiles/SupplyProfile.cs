using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Dto;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Supply;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class SupplyProfile : Profile
    {
        public SupplyProfile()
        {
            CreateMap<SupplyRequestModel, Supply>().ConvertUsing<SupplyRequestConverter>();
            CreateMap<Supply, SupplyModel>().ConvertUsing<SupplyConverter>();

            CreateMap<SuppliesFiltersRequestModel, SuppliesFiltersModel>();

            CreateMap<OperationResult<Supply>, SupplyResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplyResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<ItemsWithTotalCount<Supply>>, DataWithTotalCountResponseModel<SupplyListModel>>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SuppliesListResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}
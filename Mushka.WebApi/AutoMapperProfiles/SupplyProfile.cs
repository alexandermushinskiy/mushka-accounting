using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Dto;
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

            CreateMap<ValidationResponse<Supply>, SupplyResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplyResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            //CreateMap<ValidationResponse<IEnumerable<Supply>>, SuppliesResponseModel>()
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplyResponseResolver>())
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
            //    .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Supply>>, SuppliesListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SuppliesListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<ItemsWithTotalCount<Supply>>, DataWithTotalCountResponseModel<SupplyListModel>>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SuppliesListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
        }
    }
}
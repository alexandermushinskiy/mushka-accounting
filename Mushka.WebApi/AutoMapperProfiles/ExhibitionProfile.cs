using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class ExhibitionProfile : Profile
    {
        public ExhibitionProfile()
        {
            CreateMap<ExhibitionRequestModel, Exhibition>().ConvertUsing<ExhibitionRequestConverter>();
            CreateMap<Exhibition, ExhibitionModel>().ConvertUsing<ExhibitionConverter>();
            CreateMap<ExhibitionProduct, ExhibitionProductModel>().ConvertUsing<ExhibitionConverter>();

            CreateMap<OperationResult<Exhibition>, ExhibitionResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<IEnumerable<Exhibition>>, ExhibitionsResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<IEnumerable<Exhibition>>, ExhibitionsListResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionsListResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));

            CreateMap<OperationResult<IEnumerable<ExhibitionProduct>>, ExhibitionProductsResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionProductResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}
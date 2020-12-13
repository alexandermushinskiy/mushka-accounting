using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Customer;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>().ConvertUsing<CustomerConverter>();

            CreateMap<OperationResult<IEnumerable<Customer>>, CustomersResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CustomerResponseResolver>())
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => src.IsSuccess))
                .ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorKey)));
        }
    }
}
using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Customer;
using Mushka.WebApi.Resolvers.Customers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<OperationResult<IEnumerable<Customer>>, CustomersResponseModel>()
                .ConvertUsing<CustomersResponseConverter>();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Customer;

namespace Mushka.WebApi.Resolvers
{
    public class CustomerResponseResolver :
        IValueResolver<ValidationResponse<IEnumerable<Customer>>, CustomersResponseModel, IEnumerable<CustomerModel>>
    {
        public IEnumerable<CustomerModel> Resolve(
            ValidationResponse<IEnumerable<Customer>> source,
            CustomersResponseModel destination,
            IEnumerable<CustomerModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Customer, CustomerModel>);
    }
}
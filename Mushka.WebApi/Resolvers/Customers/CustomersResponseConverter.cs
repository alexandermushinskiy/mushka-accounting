using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Customer;

namespace Mushka.WebApi.Resolvers.Customers
{
    public class CustomersResponseConverter : ITypeConverter<OperationResult<IEnumerable<Customer>>, CustomersResponseModel>
    {
        public CustomersResponseModel Convert(
            OperationResult<IEnumerable<Customer>> source,
            CustomersResponseModel destination,
            ResolutionContext context)
        {
            return new CustomersResponseModel
            {
                Customers = source.Data?.Select(ConvertToCustomerModel)
            };
        }

        private static CustomerModel ConvertToCustomerModel(Customer customer)
            => new CustomerModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Email = customer.Email
            };
    }
}
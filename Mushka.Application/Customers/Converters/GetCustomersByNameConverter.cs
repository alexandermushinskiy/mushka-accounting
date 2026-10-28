using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Customers.Queries.GetByName;
using Mushka.Domain.Entities;

namespace Mushka.Application.Customers.Converters;

public sealed class GetCustomersByNameConverter : IGetCustomersByNameConverter
{
    public GetCustomersByNameReadModel ToReadModel(IEnumerable<Customer> customers) =>
        new GetCustomersByNameReadModel
        {
            Customers = customers.Select(ToCustomer).ToList()
        };

    private static CustomerByNameReadModel ToCustomer(Customer customer) =>
        new CustomerByNameReadModel
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Phone = customer.Phone
        };
}

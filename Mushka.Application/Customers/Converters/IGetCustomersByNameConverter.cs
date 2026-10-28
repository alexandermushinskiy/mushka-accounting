using System.Collections.Generic;
using Mushka.Application.Customers.Queries.GetByName;
using Mushka.Domain.Entities;

namespace Mushka.Application.Customers.Converters;

public interface IGetCustomersByNameConverter
{
    GetCustomersByNameReadModel ToReadModel(IEnumerable<Customer> customers);
}

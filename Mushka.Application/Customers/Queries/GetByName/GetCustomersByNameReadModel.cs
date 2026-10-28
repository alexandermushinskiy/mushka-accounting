using System;
using System.Collections.Generic;

namespace Mushka.Application.Customers.Queries.GetByName;

public sealed class GetCustomersByNameReadModel
{
    public IEnumerable<CustomerByNameReadModel> Customers { get; set; }
}

public sealed class CustomerByNameReadModel
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }
}

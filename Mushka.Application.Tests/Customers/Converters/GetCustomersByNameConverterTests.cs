using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Customers.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Customers.Converters;

public class GetCustomersByNameConverterTests
{
    private readonly GetCustomersByNameConverter _converter = new GetCustomersByNameConverter();

    [Fact]
    public void ToReadModel_MapsCustomers()
    {
        var customerId = Guid.NewGuid();
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = customerId,
                FirstName = "Ann",
                LastName = "Lee",
                Email = "ann@example.com",
                Phone = "123"
            }
        };

        var readModel = _converter.ToReadModel(customers);

        readModel.Customers.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Id = customerId,
                FirstName = "Ann",
                LastName = "Lee",
                Email = "ann@example.com",
                Phone = "123"
            });
    }
}

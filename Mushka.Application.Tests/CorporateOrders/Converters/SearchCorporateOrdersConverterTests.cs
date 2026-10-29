using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Converters;

public class SearchCorporateOrdersConverterTests
{
    private readonly SearchCorporateOrdersConverter _converter = new SearchCorporateOrdersConverter();

    [Fact]
    public void ToReadModel_MapsOrdersTotalAndAddress()
    {
        var orderId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 3, 1);
        var orders = new List<CorporateOrder>
        {
            new CorporateOrder
            {
                Id = orderId,
                CreatedOn = createdOn,
                Number = "CO-1",
                CompanyName = "Acme",
                Region = "Kyivska",
                City = "Kyiv"
            }
        };

        var readModel = _converter.ToReadModel(orders);

        readModel.Total.Should().Be(1);
        var item = readModel.Items.Should().ContainSingle().Subject;
        item.Id.Should().Be(orderId);
        item.CreatedOn.Should().Be(createdOn);
        item.OrderNumber.Should().Be("CO-1");
        item.CompanyName.Should().Be("Acme");
        item.Address.Should().Be("Kyivska, Kyiv");
    }
}

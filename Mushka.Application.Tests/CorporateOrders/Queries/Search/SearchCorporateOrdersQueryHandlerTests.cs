using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Application.CorporateOrders.Queries.Search;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Queries.Search;

public class SearchCorporateOrdersQueryHandlerTests
{
    [Fact]
    public async Task Handle_OrdersByCreatedOn_AndReturnsReadModel()
    {
        var orders = new[]
        {
            new CorporateOrder
            {
                Number = "Later",
                CreatedOn = new DateTime(2024, 5, 2),
                Region = "A",
                City = "B"
            },
            new CorporateOrder
            {
                Number = "Earlier",
                CreatedOn = new DateTime(2024, 5, 1),
                Region = "C",
                City = "D"
            }
        };
        var repository = new Mock<ICorporateOrderRepository>();
        repository
            .Setup(mock => mock.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<ICorporateOrderRepository>()).Returns(repository.Object);
        var handler = new SearchCorporateOrdersQueryHandler(storage.Object, new SearchCorporateOrdersConverter());

        var actual = await handler.Handle(new SearchCorporateOrdersQuery(), CancellationToken.None);

        actual.Total.Should().Be(2);
        actual.Items.Select(item => item.OrderNumber).Should().Equal("Earlier", "Later");
    }
}

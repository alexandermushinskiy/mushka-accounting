using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Orders.Converters;
using Mushka.Application.Orders.Queries.Search;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Orders.Queries.Search;

public class SearchOrdersQueryHandlerTests
{
    [Fact]
    public async Task Handle_SearchesWithFilter_AndReturnsPagedTotal()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 31);
        SearchOrdersFilter capturedFilter = null;
        var repository = new Mock<IOrderRepository>();
        repository
            .Setup(mock => mock.SearchAsync(It.IsAny<SearchOrdersFilter>(), It.IsAny<CancellationToken>()))
            .Callback<SearchOrdersFilter, CancellationToken>((filter, _) => capturedFilter = filter)
            .ReturnsAsync(new List<OrderSummaryDto>
            {
                new OrderSummaryDto { OrderNumber = "O-1", Address = "Kyivska, Kyiv" }
            });
        repository
            .Setup(mock => mock.GetCountAsync(It.IsAny<SearchOrdersFilter>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(8);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IOrderRepository>()).Returns(repository.Object);
        var handler = new SearchOrdersQueryHandler(storage.Object, new SearchOrdersConverter());

        var actual = await handler.Handle(
            new SearchOrdersQuery("Ann", from, to, 2, 25, "orderDate", SortOrder.Desc),
            CancellationToken.None);

        actual.Total.Should().Be(8);
        actual.Items.Should().ContainSingle(item => item.OrderNumber == "O-1");
        capturedFilter.CustomerName.Should().Be("Ann");
        capturedFilter.OrderDate.From.Should().Be(from);
        capturedFilter.OrderDate.To.Should().Be(to);
        capturedFilter.CurrentPage.Should().Be(2);
        capturedFilter.PageSize.Should().Be(25);
        capturedFilter.SortKey.Should().Be("orderDate");
        capturedFilter.SortOrder.Should().Be(SortOrder.Desc);
    }
}

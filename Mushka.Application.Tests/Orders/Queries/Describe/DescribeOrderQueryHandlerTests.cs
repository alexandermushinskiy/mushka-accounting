using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Orders.Converters;
using Mushka.Application.Orders.Queries.Describe;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Orders.Queries.Describe;

public class DescribeOrderQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _repository = new Mock<IOrderRepository>();
    private readonly DescribeOrderQueryHandler _handler;

    public DescribeOrderQueryHandlerTests()
    {
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IOrderRepository>()).Returns(_repository.Object);
        _handler = new DescribeOrderQueryHandler(storage.Object, new DescribeOrderConverter());
    }

    [Fact]
    public async Task Handle_WhenOrderIsMissing_ThrowsNotFoundException()
    {
        var id = Guid.NewGuid();
        _repository
            .Setup(repository => repository.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(new DescribeOrderQuery(id), CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.OrderNotFound);
    }

    [Fact]
    public async Task Handle_SortsAdditionalProductsFirst()
    {
        var regularId = Guid.NewGuid();
        var additionalId = Guid.NewGuid();
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Customer = new Customer { FirstName = "Ann" },
            Products = new List<OrderProduct>
            {
                Product(regularId, "Socks", false),
                Product(additionalId, "Package", true)
            }
        };
        _repository
            .Setup(repository => repository.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var actual = await _handler.Handle(new DescribeOrderQuery(order.Id), CancellationToken.None);

        actual.Products.Select(product => product.Id).Should().Equal(additionalId, regularId);
    }

    private static OrderProduct Product(Guid productId, string name, bool isAdditional) =>
        new OrderProduct
        {
            ProductId = productId,
            Product = new Product
            {
                Id = productId,
                Name = name,
                Category = new Category { IsAdditional = isAdditional }
            }
        };
}

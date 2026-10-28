using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Customers.Converters;
using Mushka.Application.Customers.Queries.GetByName;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Customers.Queries.GetByName;

public class GetCustomersByNameQueryHandlerTests
{
    [Fact]
    public async Task Handle_PassesNameToRepository_AndReturnsConvertedReadModel()
    {
        var customers = new List<Customer> { new Customer { FirstName = "Ann" } };
        var readModel = new GetCustomersByNameReadModel();
        var repository = new Mock<ICustomerRepository>();
        repository
            .Setup(mock => mock.GetByNameAsync("Ann", It.IsAny<CancellationToken>()))
            .ReturnsAsync(customers);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<ICustomerRepository>()).Returns(repository.Object);
        var converter = new Mock<IGetCustomersByNameConverter>();
        converter.Setup(mock => mock.ToReadModel(It.Is<IEnumerable<Customer>>(items => items != null))).Returns(readModel);
        var handler = new GetCustomersByNameQueryHandler(storage.Object, converter.Object);

        var actual = await handler.Handle(new GetCustomersByNameQuery("Ann"), CancellationToken.None);

        actual.Should().BeSameAs(readModel);
        repository.Verify(mock => mock.GetByNameAsync("Ann", It.IsAny<CancellationToken>()), Times.Once);
    }
}

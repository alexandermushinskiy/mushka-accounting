using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.Controllers;
using Mushka.Application.Customers.Queries.GetByName;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class CustomersControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _controller = new CustomersController(_sender.Object);
    }

    [Fact]
    public async Task Filter_SendsQueryWithName_AndReturnsReadModel()
    {
        var readModel = new GetCustomersByNameReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<GetCustomersByNameQuery>(query => query.Name == "Ann"), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetCustomersByNameReadModel>(readModel));

        var result = await _controller.Filter("Ann", CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.CorporateOrder;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.CorporateOrders;
using Mushka.Application.CorporateOrders.Commands;
using Mushka.Application.CorporateOrders.Queries.Describe;
using Mushka.Application.CorporateOrders.Queries.Search;
using Mushka.Application.CorporateOrders.Queries.ValidateNumber;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class CorporateOrdersControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<ICorporateOrderRequestConverter> _converter = new Mock<ICorporateOrderRequestConverter>();
    private readonly CorporateOrdersController _controller;

    public CorporateOrdersControllerTests()
    {
        _controller = new CorporateOrdersController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Search_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new SearchCorporateOrdersReadModel { Total = 1 };
        _sender
            .Setup(sender => sender.Send(It.IsAny<SearchCorporateOrdersQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SearchCorporateOrdersReadModel>(readModel));

        var result = await _controller.Search(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeCorporateOrderReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeCorporateOrderQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeCorporateOrderReadModel>(readModel));

        var result = await _controller.Describe(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new CorporateOrderRequestModel();
        var command = new CreateCorporateOrderCommand(null, new List<CorporateOrderProductData>());
        _converter.Setup(converter => converter.ToCreateCommand(request)).Returns(command);
        _sender
            .Setup(sender => sender.Send(command, It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Post(request, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _sender.Verify(sender => sender.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Put_SendsConvertedCommand_AndReturnsOk()
    {
        var id = Guid.NewGuid();
        var request = new CorporateOrderRequestModel();
        var command = new UpdateCorporateOrderCommand(id, null, new List<CorporateOrderProductData>());
        _converter.Setup(converter => converter.ToUpdateCommand(id, request)).Returns(command);
        _sender
            .Setup(sender => sender.Send(command, It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Put(id, request, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
        _sender.Verify(sender => sender.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_SendsCommandWithId_AndReturnsOk()
    {
        var id = Guid.NewGuid();
        _sender
            .Setup(sender => sender.Send(It.Is<DeleteCorporateOrderCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task ValidateOrderNumber_SendsQuery_AndReturnsReadModel()
    {
        var request = new ValidateCorporateOrderNumberRequestModel { OrderNumber = "CO-1" };
        var readModel = new ValidateCorporateOrderNumberReadModel { IsValid = true };
        _sender
            .Setup(sender => sender.Send(
                It.Is<ValidateCorporateOrderNumberQuery>(query => query.OrderNumber == "CO-1"),
                It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ValidateCorporateOrderNumberReadModel>(readModel));

        var result = await _controller.ValidateOrderNumber(request, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }
}

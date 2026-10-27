using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Supplier;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Suppliers;
using Mushka.Application.Suppliers.Commands;
using Mushka.Application.Suppliers.Queries.Describe;
using Mushka.Application.Suppliers.Queries.Search;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class SuppliersControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<ISupplierRequestConverter> _converter = new Mock<ISupplierRequestConverter>();
    private readonly SuppliersController _controller;

    public SuppliersControllerTests()
    {
        _controller = new SuppliersController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Search_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new SearchSuppliersReadModel { Total = 1 };
        _sender
            .Setup(sender => sender.Send(It.IsAny<SearchSuppliersQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SearchSuppliersReadModel>(readModel));

        var result = await _controller.Search(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeSupplierReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeSupplierQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeSupplierReadModel>(readModel));

        var result = await _controller.Describe(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new CreateSupplierRequestModel();
        var command = new CreateSupplierCommand(null, new List<ContactPersonData>(), new List<PaymentCardData>());
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
        var request = new UpdateSupplierRequestModel();
        var command = new UpdateSupplierCommand(id, null, new List<ContactPersonData>(), new List<PaymentCardData>());
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
            .Setup(sender => sender.Send(It.Is<DeleteSupplierCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}

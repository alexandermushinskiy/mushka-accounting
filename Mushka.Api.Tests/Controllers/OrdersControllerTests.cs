using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Order;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Orders;
using Mushka.Application.Orders.Commands;
using Mushka.Application.Orders.Queries.Describe;
using Mushka.Application.Orders.Queries.Export;
using Mushka.Application.Orders.Queries.GetDefaultProducts;
using Mushka.Application.Orders.Queries.Search;
using Mushka.Application.Orders.Queries.ValidateNumber;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<IOrderRequestConverter> _converter = new Mock<IOrderRequestConverter>();
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        _controller = new OrdersController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Search_SendsConvertedQuery_AndReturnsReadModel()
    {
        var request = new SearchOrdersRequestModel();
        var query = new SearchOrdersQuery(null, null, null, 0, 10, null, null);
        var readModel = new SearchOrdersReadModel { Total = 1 };
        _converter.Setup(converter => converter.ToSearchQuery(request)).Returns(query);
        _sender
            .Setup(sender => sender.Send(query, It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SearchOrdersReadModel>(readModel));

        var result = await _controller.Search(request, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetDefaultProducts_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new GetDefaultOrderProductsReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetDefaultOrderProductsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetDefaultOrderProductsReadModel>(readModel));

        var result = await _controller.GetDefaultProducts(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeOrderReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeOrderQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeOrderReadModel>(readModel));

        var result = await _controller.Describe(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new OrderRequestModel();
        var command = new CreateOrderCommand(null, null, new List<OrderProductData>());
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
        var request = new OrderRequestModel();
        var command = new UpdateOrderCommand(id, null, null, new List<OrderProductData>());
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
            .Setup(sender => sender.Send(It.Is<DeleteOrderCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task ValidateOrderNumber_SendsQuery_AndReturnsReadModel()
    {
        var request = new ValidateOrderNumberRequestModel { OrderNumber = "O-1" };
        var readModel = new ValidateOrderNumberReadModel { IsValid = true };
        _sender
            .Setup(sender => sender.Send(
                It.Is<ValidateOrderNumberQuery>(query => query.OrderNumber == "O-1"),
                It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ValidateOrderNumberReadModel>(readModel));

        var result = await _controller.ValidateOrderNumber(request, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Export_SendsQuery_AndReturnsFile()
    {
        var orderId = Guid.NewGuid();
        var request = new ExportOrdersRequestModel { Title = "March", OrderIds = new[] { orderId } };
        var content = new MemoryStream();
        var readModel = new ExportOrdersReadModel
        {
            Name = "mushka_export_orders.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content = content
        };
        _sender
            .Setup(sender => sender.Send(
                It.Is<ExportOrdersQuery>(query => query.Title == "March" && query.OrderIds.Contains(orderId)),
                It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ExportOrdersReadModel>(readModel));

        var result = await _controller.Export(request, CancellationToken.None);

        var file = result.Should().BeOfType<FileStreamResult>().Subject;
        file.FileStream.Should().BeSameAs(content);
        file.FileDownloadName.Should().Be(readModel.Name);
        file.ContentType.Should().Be(readModel.ContentType);
    }
}

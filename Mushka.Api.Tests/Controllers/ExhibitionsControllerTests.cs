using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Exhibition;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Exhibitions;
using Mushka.Application.Exhibitions.Commands;
using Mushka.Application.Exhibitions.Queries.Describe;
using Mushka.Application.Exhibitions.Queries.GetDefaultProducts;
using Mushka.Application.Exhibitions.Queries.Search;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class ExhibitionsControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<IExhibitionRequestConverter> _converter = new Mock<IExhibitionRequestConverter>();
    private readonly ExhibitionsController _controller;

    public ExhibitionsControllerTests()
    {
        _controller = new ExhibitionsController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Search_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new SearchExhibitionsReadModel { Total = 1 };
        _sender
            .Setup(sender => sender.Send(It.IsAny<SearchExhibitionsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SearchExhibitionsReadModel>(readModel));

        var result = await _controller.Search(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeExhibitionReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeExhibitionQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeExhibitionReadModel>(readModel));

        var result = await _controller.Describe(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetDefaultProducts_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new GetDefaultExhibitionProductsReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetDefaultExhibitionProductsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetDefaultExhibitionProductsReadModel>(readModel));

        var result = await _controller.GetDefaultProducts(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new ExhibitionRequestModel();
        var command = new CreateExhibitionCommand(null, new List<ExhibitionProductData>());
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
        var request = new ExhibitionRequestModel();
        var command = new UpdateExhibitionCommand(id, null, new List<ExhibitionProductData>());
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
            .Setup(sender => sender.Send(It.Is<DeleteExhibitionCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}

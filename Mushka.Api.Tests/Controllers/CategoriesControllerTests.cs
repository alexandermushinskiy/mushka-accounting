using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Category;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Categories;
using Mushka.Application.Categories.Commands;
using Mushka.Application.Categories.Queries.Describe;
using Mushka.Application.Categories.Queries.Search;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<ICategoryRequestConverter> _converter = new Mock<ICategoryRequestConverter>();
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _controller = new CategoriesController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Search_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new SearchCategoriesReadModel { Total = 1 };
        _sender
            .Setup(sender => sender.Send(It.IsAny<SearchCategoriesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SearchCategoriesReadModel>(readModel));

        var result = await _controller.Search(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeCategoryReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeCategoryQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeCategoryReadModel>(readModel));

        var result = await _controller.Describe(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new CreateCategoryRequestModel();
        var command = new CreateCategoryCommand(new CategoryData("Socks", true, false));
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
        var request = new UpdateCategoryRequestModel();
        var command = new UpdateCategoryCommand(id, new CategoryData("Socks", true, false));
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
            .Setup(sender => sender.Send(It.Is<DeleteCategoryCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }
}

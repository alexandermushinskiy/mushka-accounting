using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Product;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Products;
using Mushka.Application.Products.Commands;
using Mushka.Application.Products.Queries.Describe;
using Mushka.Application.Products.Queries.GetByCategory;
using Mushka.Application.Products.Queries.GetCostPrice;
using Mushka.Application.Products.Queries.GetSizes;
using Mushka.Application.Products.Queries.Select;
using Xunit;

namespace Mushka.Api.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<ISender> _sender = new Mock<ISender>();
    private readonly Mock<IProductRequestConverter> _converter = new Mock<IProductRequestConverter>();
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new ProductsController(_sender.Object, _converter.Object);
    }

    [Fact]
    public async Task Select_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new SelectProductsReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<SelectProductsQuery>(query => query.InStock), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<SelectProductsReadModel>(readModel));

        var result = await _controller.Select(true, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetByCategoryId_SendsQueryWithCategoryId_AndReturnsReadModel()
    {
        var categoryId = Guid.NewGuid();
        var readModel = new GetProductsByCategoryReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<GetProductsByCategoryQuery>(query => query.CategoryId == categoryId), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetProductsByCategoryReadModel>(readModel));

        var result = await _controller.GetByCategoryId(categoryId, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetById_SendsQueryWithId_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new DescribeProductReadModel();
        _sender
            .Setup(sender => sender.Send(It.Is<DescribeProductQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DescribeProductReadModel>(readModel));

        var result = await _controller.GetById(id, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task GetCostPrice_SendsQuery_AndReturnsReadModel()
    {
        var id = Guid.NewGuid();
        var readModel = new GetProductCostPriceReadModel { CostPrice = 4 };
        _sender
            .Setup(sender => sender.Send(It.Is<GetProductCostPriceQuery>(query => query.Id == id && query.ProductsCount == 2), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetProductCostPriceReadModel>(readModel));

        var result = await _controller.GetCostPrice(id, 2, CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }

    [Fact]
    public async Task Post_SendsConvertedCommand_AndReturnsOk()
    {
        var request = new CreateProductRequestModel();
        var command = new CreateProductCommand(new ProductData("Socks", "SK-1", null, Guid.NewGuid(), null, false));
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
        var request = new UpdateProductRequestModel();
        var command = new UpdateProductCommand(id, new ProductData("Socks", "SK-1", null, Guid.NewGuid(), null, false));
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
            .Setup(sender => sender.Send(It.Is<DeleteProductCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Unit>(Unit.Value));

        var result = await _controller.Delete(id, CancellationToken.None);

        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task GetSizes_SendsQuery_AndReturnsReadModel()
    {
        var readModel = new GetProductSizesReadModel();
        _sender
            .Setup(sender => sender.Send(It.IsAny<GetProductSizesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetProductSizesReadModel>(readModel));

        var result = await _controller.GetSizes(CancellationToken.None);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(readModel);
    }
}

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
using Mushka.Api.ClientModels.Product;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Products;
using Mushka.Application.Products.Commands;
using Mushka.Application.Products.Queries.Describe;
using Mushka.Application.Products.Queries.Export;
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

    [Fact]
    public async Task Export_SendsQuery_AndReturnsFile()
    {
        var productId = Guid.NewGuid();
        var request = new ExportProductsRequestModel
        {
            Title = "Stock",
            ProductIds = new[] { productId }
        };
        var content = new MemoryStream();
        var readModel = new ExportProductsReadModel
        {
            Name = "mushka_export_products.xlsx",
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            Content = content
        };
        _sender
            .Setup(sender => sender.Send(It.Is<ExportProductsQuery>(query => query.Title == "Stock" && query.ProductIds.Single() == productId), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<ExportProductsReadModel>(readModel));

        var result = await _controller.Export(request, CancellationToken.None);

        var file = result.Should().BeOfType<FileStreamResult>().Subject;
        file.FileStream.Should().BeSameAs(content);
        file.FileDownloadName.Should().Be(readModel.Name);
        file.ContentType.Should().Be(readModel.ContentType);
    }
}

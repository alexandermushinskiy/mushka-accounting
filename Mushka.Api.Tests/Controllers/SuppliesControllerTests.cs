using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Mushka.Api.ClientModels.Supply;
using Mushka.Api.Controllers;
using Mushka.Api.Converters.Supplies;
using Mushka.Application.Supplies.Commands;
using Mushka.Application.Supplies.Queries.Describe;
using Mushka.Application.Supplies.Queries.Export;
using Mushka.Application.Supplies.Queries.Search;
using Xunit;

namespace Mushka.Api.Tests.Controllers
{
    public class SuppliesControllerTests
    {
        private readonly Mock<ISender> _sender = new Mock<ISender>();
        private readonly Mock<ISupplyRequestConverter> _converter = new Mock<ISupplyRequestConverter>();
        private readonly SuppliesController _controller;

        public SuppliesControllerTests()
        {
            _controller = new SuppliesController(_sender.Object, _converter.Object);
        }

        [Fact]
        public async Task Search_SendsQuery_AndReturnsReadModel()
        {
            var productId = Guid.NewGuid();
            var request = new SearchSuppliesRequestModel { SearchKey = "acme", ProductId = productId };
            var readModel = new SearchSuppliesReadModel { Total = 4 };
            _sender
                .Setup(sender => sender.Send(
                    It.Is<SearchSuppliesQuery>(query => query.SearchKey == "acme" && query.ProductId == productId),
                    It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<SearchSuppliesReadModel>(readModel));

            var result = await _controller.Search(request, CancellationToken.None);

            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(readModel);
        }

        [Fact]
        public async Task Describe_SendsQueryWithId_AndReturnsReadModel()
        {
            var id = Guid.NewGuid();
            var readModel = new DescribeSupplyReadModel();
            _sender
                .Setup(sender => sender.Send(It.Is<DescribeSupplyQuery>(query => query.Id == id), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<DescribeSupplyReadModel>(readModel));

            var result = await _controller.Describe(id, CancellationToken.None);

            var ok = result.Should().BeOfType<OkObjectResult>().Subject;
            ok.Value.Should().BeSameAs(readModel);
        }

        [Fact]
        public async Task Post_SendsConvertedCommand_AndReturnsOk()
        {
            var request = new SupplyRequestModel();
            var command = new CreateSupplyCommand(null, new List<SupplyProductCommandModel>());
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
            var request = new SupplyRequestModel();
            var command = new UpdateSupplyCommand(id, null, new List<SupplyProductCommandModel>());
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
                .Setup(sender => sender.Send(It.Is<DeleteSupplyCommand>(command => command.Id == id), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<Unit>(Unit.Value));

            var result = await _controller.Delete(id, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Export_SendsQuery_AndReturnsFile()
        {
            var supplyId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var request = new ExportSuppliesRequestModel
            {
                SupplyIds = new[] { supplyId },
                ProductIds = new[] { productId }
            };
            var content = new MemoryStream();
            var readModel = new ExportSuppliesReadModel
            {
                Name = "mushka_export_supply_products.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                Content = content
            };
            _sender
                .Setup(sender => sender.Send(It.IsAny<ExportSuppliesQuery>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<ExportSuppliesReadModel>(readModel));

            var result = await _controller.Export(request, CancellationToken.None);

            var file = result.Should().BeOfType<FileStreamResult>().Subject;
            file.FileStream.Should().BeSameAs(content);
            file.FileDownloadName.Should().Be(readModel.Name);
            file.ContentType.Should().Be(readModel.ContentType);
        }
    }
}

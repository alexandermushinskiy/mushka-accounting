using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Products.Export;
using Mushka.Application.Products.Queries.Export;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Products.Queries.Export;

public class ExportProductsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExportsRequestedProducts()
    {
        var productId = Guid.NewGuid();
        var products = new[] { new Product { Id = productId, Name = "Socks" } };
        IEnumerable<Product> exportedProducts = null;
        var repository = new Mock<IProductRepository>();
        repository
            .Setup(mock => mock.GetForExportAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);
        var exporter = new Mock<IProductExcelExporter>();
        exporter
            .Setup(mock => mock.Export("Stock", It.IsAny<IEnumerable<Product>>()))
            .Callback<string, IEnumerable<Product>>((_, exported) => exportedProducts = exported)
            .Returns(Stream.Null);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(repository.Object);
        var handler = new ExportProductsQueryHandler(storage.Object, exporter.Object);

        var actual = await handler.Handle(new ExportProductsQuery("Stock", new[] { productId }), CancellationToken.None);

        actual.Name.Should().Be("mushka_export_products.xlsx");
        actual.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        actual.Content.Should().BeSameAs(Stream.Null);
        exportedProducts.Should().BeSameAs(products);
    }
}

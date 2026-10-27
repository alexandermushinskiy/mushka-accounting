using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Supplies.Export;
using Mushka.Application.Supplies.Queries.Export;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Queries.Export
{
    public class ExportSuppliesQueryHandlerTests
    {
        private readonly Mock<ISupplyRepository> _supplyRepository = new Mock<ISupplyRepository>();
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<ISupplyExcelExporter> _exporter = new Mock<ISupplyExcelExporter>();
        private readonly ExportSuppliesQueryHandler _handler;

        public ExportSuppliesQueryHandlerTests()
        {
            var storage = new Mock<IStorage>();
            storage.Setup(mock => mock.GetRepository<ISupplyRepository>()).Returns(_supplyRepository.Object);
            storage.Setup(mock => mock.GetRepository<IProductRepository>()).Returns(_productRepository.Object);
            _handler = new ExportSuppliesQueryHandler(storage.Object, _exporter.Object);
        }

        [Fact]
        public async Task Handle_OrdersSuppliesAndProducts_AndUsesRequestedProductIds()
        {
            var supplierA = new Supplier { Name = "Alpha" };
            var supplierB = new Supplier { Name = "Beta" };
            var supplies = new[]
            {
                new Supply { Supplier = supplierB, RequestDate = new DateTime(2024, 1, 2), Products = new List<SupplyProduct>() },
                new Supply { Supplier = supplierA, RequestDate = new DateTime(2024, 3, 1), Products = new List<SupplyProduct>() },
                new Supply { Supplier = supplierA, RequestDate = new DateTime(2024, 1, 1), Products = new List<SupplyProduct>() }
            };
            var productId = Guid.NewGuid();
            var products = new[]
            {
                new Product { Id = productId, Name = "Beta", VendorCode = "2" },
                new Product { Id = Guid.NewGuid(), Name = "Alpha", VendorCode = "9" },
                new Product { Id = Guid.NewGuid(), Name = "Beta", VendorCode = "1" }
            };
            List<Supply> exportedSupplies = null;
            List<Product> exportedProducts = null;
            _supplyRepository
                .Setup(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Supply, bool>>>(),
                    It.Is<string[]>(includes => includes.Contains(nameof(Supply.Supplier)) && includes.Contains(nameof(Supply.Products))),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(supplies);
            _productRepository
                .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);
            _exporter
                .Setup(exporter => exporter.Export(It.IsAny<IEnumerable<Supply>>(), It.IsAny<IEnumerable<Product>>()))
                .Callback<IEnumerable<Supply>, IEnumerable<Product>>((exported, exportedProductList) =>
                {
                    exportedSupplies = exported.ToList();
                    exportedProducts = exportedProductList.ToList();
                })
                .Returns(Stream.Null);

            var actual = await _handler.Handle(
                new ExportSuppliesQuery(new[] { Guid.NewGuid() }, new[] { productId }),
                CancellationToken.None);

            actual.Name.Should().Be("mushka_export_supply_products.xlsx");
            actual.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            actual.Content.Should().BeSameAs(Stream.Null);
            exportedSupplies.Select(supply => supply.RequestDate).Should().Equal(
                new DateTime(2024, 1, 1),
                new DateTime(2024, 3, 1),
                new DateTime(2024, 1, 2));
            exportedProducts.Select(product => product.VendorCode).Should().Equal("9", "1", "2");
            _productRepository.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenProductIdsAreEmpty_UsesProductsFromSupplies()
        {
            var productId = Guid.NewGuid();
            var supplies = new[]
            {
                new Supply
                {
                    Supplier = new Supplier { Name = "Acme" },
                    Products = new List<SupplyProduct> { new SupplyProduct { ProductId = productId } }
                }
            };
            Expression<Func<Product, bool>> captured = null;
            _supplyRepository
                .Setup(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Supply, bool>>>(),
                    It.IsAny<string[]>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(supplies);
            _productRepository
                .Setup(repository => repository.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .Callback<Expression<Func<Product, bool>>, CancellationToken>((predicate, _) => captured = predicate)
                .ReturnsAsync(new[] { new Product { Id = productId, Name = "Sock", VendorCode = "1" } });
            _exporter
                .Setup(exporter => exporter.Export(It.IsAny<IEnumerable<Supply>>(), It.IsAny<IEnumerable<Product>>()))
                .Returns(Stream.Null);

            await _handler.Handle(new ExportSuppliesQuery(new[] { Guid.NewGuid() }, Array.Empty<Guid>()), CancellationToken.None);

            captured.Compile().Invoke(new Product { Id = productId }).Should().BeTrue();
            captured.Compile().Invoke(new Product { Id = Guid.NewGuid() }).Should().BeFalse();
            _productRepository.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNoProductsCanBeResolved_LoadsAllProducts()
        {
            var supplies = new[]
            {
                new Supply
                {
                    Supplier = new Supplier { Name = "Acme" },
                    Products = new List<SupplyProduct>()
                }
            };
            _supplyRepository
                .Setup(repository => repository.GetAsync(
                    It.IsAny<Expression<Func<Supply, bool>>>(),
                    It.IsAny<string[]>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(supplies);
            _productRepository
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[] { new Product { Name = "Sock", VendorCode = "1" } });
            _exporter
                .Setup(exporter => exporter.Export(It.IsAny<IEnumerable<Supply>>(), It.IsAny<IEnumerable<Product>>()))
                .Returns(Stream.Null);

            await _handler.Handle(new ExportSuppliesQuery(Array.Empty<Guid>(), Array.Empty<Guid>()), CancellationToken.None);

            _productRepository.Verify(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.ExternalApps;
using Mushka.Service.Services;
using Mushka.Tests.Common;
using Xunit;

namespace Mushka.Tests.Service.Services
{
    public class SupplyServiceTest : ServiceTestBase
    {
        private const string CategoryName = Component.Service + nameof(SupplyService);
        private const string GetAllAsyncMethodName = nameof(SupplyService.GetAllAsync) + ". ";
        private const string GetByIdAsyncMethodName = nameof(SupplyService.GetByIdAsync) + ". ";
        private const string AddAsyncMethodName = nameof(SupplyService.AddAsync) + ". ";
        private const string UpdateAsyncMethodName = nameof(SupplyService.UpdateAsync) + ". ";
        private const string DeleteAsyncMethodName = nameof(SupplyService.DeleteAsync) + ". ";

        private const string SuppliesRetrievedMessage = "Supplies were successfully retrieved.";
        private const string NoSuppliesFoundMessage = "No supplies found.";
        private const string SupplierName = "Supplier";
        private static readonly Guid SupplyId = Guid.Parse("00000000000000000000000000000001");
        private static readonly Guid ProductId = Guid.Parse("00000000000000000000000000000002");
        private static readonly Guid OtherProductId = Guid.Parse("00000000000000000000000000000004");
        private static readonly string SupplyRetrievedMessage = $"Supply with id {SupplyId} was successfully retrieved.";
        private static readonly string SupplyDeletedMessage = $"Supply with id {SupplyId} was successfully deleted.";
        private static readonly string SupplyNotFoundMessage = $"Supply with id {SupplyId} is not found.";
        private static readonly string SupplyCreatedMessage = $"Supply with id {SupplyId} was successfully added.";
        private static readonly string SupplyUpdatedMessage = $"Supply with id {SupplyId} was successfully updated.";
        private static readonly string ProductNotFoundMessage = $"Product with id {ProductId} is not found.";

        private readonly Mock<IStorage> storageMock;
        private readonly Mock<IProductRepository> productRepositoryMock;
        private readonly Mock<ISupplyRepository> supplyRepositoryMock;
        private readonly Mock<IExcelService> excelServiceMock;
        private readonly SupplyService supplyService;

        public SupplyServiceTest()
        {
            supplyRepositoryMock = MockRepository.Create<ISupplyRepository>();
            productRepositoryMock = MockRepository.Create<IProductRepository>();
            excelServiceMock = MockRepository.Create<IExcelService>();

            var loggerFactory = MockRepository
                .Create<ILoggerFactory>()
                .Setup(lf => lf.CreateLogger(nameof(SupplyService)), LoggerMock.Object)
                .Object;

            storageMock = MockRepository
                .Create<IStorage>()
                .Setup(str => str.GetRepository<IProductRepository>(), productRepositoryMock.Object)
                .Setup(str => str.GetRepository<ISupplyRepository>(), supplyRepositoryMock.Object);

            supplyService = new SupplyService(
                storageMock.Object,
                excelServiceMock.Object,
                loggerFactory);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName)]
        public async Task GetAllAsyncTest()
        {
            var supplies = new[] { CreateSupply(new[] { CreateSupplyProduct(10) }) };

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), supplies);

            var actual = await supplyService.GetAllAsync();

            var expected = CreateValidValidationResponse(supplies, SuppliesRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName + "No supplies found")]
        public async Task GetAllAsyncNoSuppliesFoundTest()
        {
            supplyRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Supply>());

            var actual = await supplyService.GetAllAsync();

            var expected = CreateValidValidationResponse(Enumerable.Empty<Supply>(), NoSuppliesFoundMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(10) });

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), supply);

            var actual = await supplyService.GetByIdAsync(SupplyId);

            var expected = CreateValidValidationResponse(supply, SupplyRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName + "Supply is not found")]
        public async Task GetByIdAsyncSupplyNotFoundTest()
        {
            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), null);

            var actual = await supplyService.GetByIdAsync(SupplyId);

            var expected = CreateWarningValidationResponse<Supply>(SupplyNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName)]
        public async Task AddAsyncTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(10) });
            var product = CreateProduct(20);
            const int ExpectedQuantity = 30;

            supplyRepositoryMock
                .Setup(repo => repo.Add(supply), supply);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await supplyService.AddAsync(supply);

            var expected = CreateValidValidationResponse(supply, SupplyCreatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "Supply product is not found")]
        public async Task AddAsyncSupplyProductNotFoundTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(5) });
            
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await supplyService.AddAsync(supply);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName)]
        public async Task UpdateAsyncTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(7) });
            var storedSupply = CreateSupply(new[] { CreateSupplyProduct(12) });
            var product = CreateProduct(12);
            const int ExpectedProductQuantity = 7;

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), storedSupply)
                .Setup(repo => repo.Update(It.Is<Supply>(sup => sup.Id == SupplyId)), supply);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await supplyService.UpdateAsync(supply);

            var expected = CreateValidValidationResponse(supply, SupplyUpdatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Supply is not found")]
        public async Task UpdateAsyncSupplyNotFoundTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(5) });

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), null);

            var actual = await supplyService.UpdateAsync(supply);

            var expected = CreateWarningValidationResponse<Supply>(SupplyNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Supply product is not found")]
        public async Task UpdateAsyncSupplyProductNotFoundTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(5) });

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), supply);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await supplyService.UpdateAsync(supply);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Supply has new product")]
        public async Task UpdateAsyncSupplyNewAnotherProductTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(SupplyId, ProductId, 5) });
            var storedSupply = CreateSupply(new[] { CreateSupplyProduct(SupplyId, OtherProductId, 7) });
            var updatedSupply = CreateSupply(new[] { CreateSupplyProduct(SupplyId, ProductId, 5) });
            var otherProduct = CreateProduct(OtherProductId, 22);
            var product = CreateProduct(ProductId, 33);
            const int ExpectedReturnedProductQuantity = 15;
            const int ExpectedSuppliedProductQuantity = 38;

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), storedSupply)
                .Setup(repo => repo.Update(It.Is<Supply>(ord => ord.Id == SupplyId)), updatedSupply);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OtherProductId, default(CancellationToken)), otherProduct)
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedSuppliedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedSuppliedProductQuantity))
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == OtherProductId && prod.Quantity == ExpectedReturnedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == OtherProductId && prod.Quantity == ExpectedReturnedProductQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await supplyService.UpdateAsync(supply);

            var expected = CreateValidValidationResponse(updatedSupply, SupplyUpdatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName)]
        public async Task DeleteAsyncTest()
        {
            var storedSupply = CreateSupply(new[] { CreateSupplyProduct(3) });
            var product = CreateProduct(7);
            const int ExpectedProductQuantity = 4;

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), storedSupply)
                .Setup(repo => repo.Delete(storedSupply), storedSupply);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await supplyService.DeleteAsync(SupplyId);

            var expected = CreateValidValidationResponse(storedSupply, SupplyDeletedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Supply is not found")]
        public async Task DeleteAsyncSupplyNotFoundTest()
        {
            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), null);

            var actual = await supplyService.DeleteAsync(SupplyId);

            var expected = CreateWarningValidationResponse<Supply>(SupplyNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Supply product is not found")]
        public async Task DeleteAsyncSupplyProductNotFoundTest()
        {
            var supply = CreateSupply(new[] { CreateSupplyProduct(5) });

            supplyRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(SupplyId, default(CancellationToken)), supply);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await supplyService.DeleteAsync(SupplyId);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        private static Supply CreateSupply(ICollection<SupplyProduct> supplyProducts) =>
            new Supply
            {
                Id = SupplyId,
                Supplier = new Supplier
                {
                    Name = SupplierName
                },
                Products = supplyProducts
            };

        private static SupplyProduct CreateSupplyProduct(int quantity) =>
            new SupplyProduct
            {
                SupplyId = SupplyId,
                ProductId = ProductId,
                Quantity = quantity
            };

        private static SupplyProduct CreateSupplyProduct(Guid supplyId, Guid productId, int quantity) =>
            new SupplyProduct
            {
                SupplyId = supplyId,
                ProductId = productId,
                Quantity = quantity
            };

        private static Product CreateProduct(int quantity) =>
            new Product
            {
                Id = ProductId,
                Quantity = quantity
            };

        private static Product CreateProduct(Guid productId, int quantity) =>
            new Product
            {
                Id = productId,
                Quantity = quantity
            };
    }
}
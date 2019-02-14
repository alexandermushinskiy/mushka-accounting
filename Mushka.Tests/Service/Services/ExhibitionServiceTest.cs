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
using Mushka.Service.Services;
using Mushka.Tests.Common;
using Xunit;

namespace Mushka.Tests.Service.Services
{
    public class ExhibitionServiceTest : ServiceTestBase
    {
        private const string CategoryName = Component.Service + nameof(ExhibitionService);
        private const string GetAllAsyncMethodName = nameof(ExhibitionService.GetAllAsync) + ". ";
        private const string GetByIdAsyncMethodName = nameof(ExhibitionService.GetByIdAsync) + ". ";
        private const string AddAsyncMethodName = nameof(ExhibitionService.AddAsync) + ". ";
        private const string UpdateAsyncMethodName = nameof(ExhibitionService.UpdateAsync) + ". ";
        private const string DeleteAsyncMethodName = nameof(ExhibitionService.DeleteAsync) + ". ";

        private const string ExhibitionsRetrievedMessage = "Exhibitions were successfully retrieved.";
        private const string NoExhibitionsFoundMessage = "No exhibitions found.";
        private static readonly Guid ExhibitionId = Guid.Parse("00000000000000000000000000000001");
        private static readonly Guid ProductId = Guid.Parse("00000000000000000000000000000002");
        private static readonly string ExhibitionRetrievedMessage = $"Exhibition with id {ExhibitionId} was successfully retrieved.";
        private static readonly string ExhibitionDeletedMessage = $"Exhibition with id {ExhibitionId} was successfully deleted.";
        private static readonly string ExhibitionNotFoundMessage = $"Exhibition with id {ExhibitionId} is not found.";
        private static readonly string ProductNotFoundMessage = $"Product with id {ProductId} is not found.";
        private static readonly string ExhibitionCreatedMessage = $"Exhibition with id {ExhibitionId} was successfully added.";
        private static readonly string ExhibitionUpdatedMessage = $"Exhibition with id {ExhibitionId} was successfully updated.";
        private static readonly string ProductNotEnoughMessage = $"Product with id {ProductId} is not enough in stock.";

        private readonly Mock<IStorage> storageMock;
        private readonly Mock<IExhibitionRepository> exhibitionRepositoryMock;
        private readonly Mock<IProductRepository> productRepositoryMock;
        private readonly ExhibitionService exhibitionService;

        public ExhibitionServiceTest()
        {
            productRepositoryMock = MockRepository.Create<IProductRepository>();
            exhibitionRepositoryMock = MockRepository.Create<IExhibitionRepository>();

            var loggerFactory = MockRepository
                .Create<ILoggerFactory>()
                .Setup(lf => lf.CreateLogger(nameof(ExhibitionService)), LoggerMock.Object)
                .Object;

            storageMock = MockRepository
                .Create<IStorage>()
                .Setup(str => str.GetRepository<IProductRepository>(), productRepositoryMock.Object)
                .Setup(str => str.GetRepository<IExhibitionRepository>(), exhibitionRepositoryMock.Object);

            exhibitionService = new ExhibitionService(
                storageMock.Object,
                loggerFactory);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName)]
        public async Task GetAllAsyncTest()
        {
            var exhibitions = new[] { CreateExhibition(new[] { CreateExhibitionProduct(10) }) };

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), exhibitions);

            var actual = await exhibitionService.GetAllAsync();

            var expected = CreateValidValidationResponse(exhibitions, ExhibitionsRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName + "No exhibitions found")]
        public async Task GetAllAsyncNoOrdersFoundTest()
        {
            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Exhibition>());

            var actual = await exhibitionService.GetAllAsync();

            var expected = CreateValidValidationResponse(Enumerable.Empty<Exhibition>(), NoExhibitionsFoundMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(10) });

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), exhibition);

            var actual = await exhibitionService.GetByIdAsync(ExhibitionId);

            var expected = CreateValidValidationResponse(exhibition, ExhibitionRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncExhibitionNotFoundTest()
        {
            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), null);

            var actual = await exhibitionService.GetByIdAsync(ExhibitionId);

            var expected = CreateWarningValidationResponse<Exhibition>(ExhibitionNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName)]
        public async Task AddAsyncTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(10) });
            var product = CreateProduct(20);
            const int ExpectedQuantity = 10;

            exhibitionRepositoryMock
                .Setup(repo => repo.Add(exhibition), exhibition);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await exhibitionService.AddAsync(exhibition);

            var expected = CreateValidValidationResponse(exhibition, ExhibitionCreatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "Exhibition product is not found")]
        public async Task AddAsyncOrderProductNotFoundTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(5) });
            
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await exhibitionService.AddAsync(exhibition);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "Products quantity is not enough")]
        public async Task AddAsyncProductsQuantityNotEnoughTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(10) });
            var product = CreateProduct(5);
            
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product);

            var actual = await exhibitionService.AddAsync(exhibition);

            var expected = CreateWarningValidationResponse<Product>(ProductNotEnoughMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName)]
        public async Task UpdateAsyncTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(5) });
            var storedExhibition = CreateExhibition(new[] { CreateExhibitionProduct(10) });
            var updatedExhibition = CreateExhibition(new[] { CreateExhibitionProduct(25) });
            var product = CreateProduct(20);
            const int ExpectedProductQuantity = 25;

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), storedExhibition)
                .Setup(repo => repo.Update(It.Is<Exhibition>(ord => ord.Id == ExhibitionId)), updatedExhibition);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await exhibitionService.UpdateAsync(exhibition);

            var expected = CreateValidValidationResponse(updatedExhibition, ExhibitionUpdatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Exhibition is not found")]
        public async Task UpdateAsyncExhibitionNotFoundTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(5) });

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), null);

            var actual = await exhibitionService.UpdateAsync(exhibition);

            var expected = CreateWarningValidationResponse<Exhibition>(ExhibitionNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Order product is not found")]
        public async Task UpdateAsyncOrderProductNotFoundTest()
        {
            var exhibition = CreateExhibition(new[] { CreateExhibitionProduct(5) });

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), exhibition);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await exhibitionService.UpdateAsync(exhibition);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName)]
        public async Task DeleteAsyncTest()
        {
            var storedExhibition = CreateExhibition(new[] { CreateExhibitionProduct(3) });
            var product = CreateProduct(7);
            const int ExpectedProductQuantity = 10;

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), storedExhibition)
                .Setup(repo => repo.Delete(storedExhibition), storedExhibition);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await exhibitionService.DeleteAsync(ExhibitionId);

            var expected = CreateValidValidationResponse(storedExhibition, ExhibitionDeletedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Exhibition is not found")]
        public async Task DeleteAsyncExhibitionNotFoundTest()
        {
            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), null);

            var actual = await exhibitionService.DeleteAsync(ExhibitionId);

            var expected = CreateWarningValidationResponse<Exhibition>(ExhibitionNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Exhibition product is not found")]
        public async Task DeleteAsyncExhibitionProductNotFoundTest()
        {
            var storedExhibition = CreateExhibition(new[] { CreateExhibitionProduct(5) });

            exhibitionRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ExhibitionId, default(CancellationToken)), storedExhibition);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await exhibitionService.DeleteAsync(ExhibitionId);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        private static Exhibition CreateExhibition(ICollection<ExhibitionProduct> exhibitionProducts) =>
            new Exhibition
            {
                Id = ExhibitionId,
                Products = exhibitionProducts
            };


        private static ExhibitionProduct CreateExhibitionProduct(int quantity) =>
            new ExhibitionProduct
            {
                ExhibitionId = ExhibitionId,
                ProductId = ProductId,
                Quantity = quantity
            };

        private static Product CreateProduct(int quantity) =>
            new Product
            {
                Id = ProductId,
                Quantity = quantity
            };
    }
}
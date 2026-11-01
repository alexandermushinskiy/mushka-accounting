using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Services;
using Mushka.Tests.Common;
using Xunit;

namespace Mushka.Tests.Service.Services
{
    public class ProductServiceTest : ServiceTestBase
    {
        private const string CategoryName = Component.Service + nameof(ProductService);
        private const string GetAllAsyncMethodName = nameof(ProductService.GetAllAsync) + ". ";
        private const string GetByIdAsyncMethodName = nameof(ProductService.GetByIdAsync) + ". ";
        private const string GetCostPriceAsyncMethodName = nameof(ProductService.GetCostPriceAsync) + ". ";
        private const string GetByCriteriaAsyncMethodName = nameof(ProductService.GetByCriteriaAsync) + ". ";
        private const string GetByCategoryAsyncMethodName = nameof(ProductService.GetByCategoryAsync) + ". ";
        private const string AddAsyncMethodName = nameof(ProductService.AddAsync) + ". ";
        private const string UpdateAsyncMethodName = nameof(ProductService.UpdateAsync) + ". ";
        private const string DeleteAsyncMethodName = nameof(ProductService.DeleteAsync) + ". ";
        private const string GetSizesAsyncMethodName = nameof(ProductService.GetSizesAsync) + ". ";
        private const string GetInStockAsyncMethodName = nameof(ProductService.GetInStockAsync) + ". ";

        private const string ProductsRetrievedMessage = "Products were successfully retrieved.";
        private const string NoProductsFoundMessage = "No products found.";
        public const string ProductVendorCode = "VNDRCD";
        private static readonly Guid ProductId = Guid.Parse("00000000000000000000000000000001");
        private static readonly Guid CategoryId= Guid.Parse("00000000000000000000000000000022");
        private static readonly string ProductRetrievedMessage = $"Product with id {ProductId} was successfully retrieved.";
        private static readonly string ProductDeletedMessage = $"Product with id {ProductId} was successfully deleted.";
        private static readonly string ProductAddedMessage = $"Product with id {ProductId} was successfully added.";
        private static readonly string ProductUpdatedMessage = $"Product with id {ProductId} was successfully updated.";
        private static readonly string ProductNotFoundMessage = $"Product with id {ProductId} is not found.";
        private static readonly string CategoryNotFoundMessage = $"Category with id {CategoryId} is not found.";
        private static readonly string VendorExistMessage = $"Product with the vendor code {ProductVendorCode} is already existed.";
        private static readonly string ProductCostPriceRetrievedMessage = $"Cost price for product with id {ProductId} was successfully retrieved.";

        private readonly Mock<IStorage> storageMock;
        private readonly Mock<IProductRepository> productRepositoryMock;
        private readonly Mock<ICategoryRepository> categoryRepositoryMock;
        private readonly Mock<ICostPriceProvider> costPriceProviderMock;
        private readonly ProductService productService;

        public ProductServiceTest()
        {
            productRepositoryMock = MockRepository.Create<IProductRepository>();
            categoryRepositoryMock = MockRepository.Create<ICategoryRepository>();
            costPriceProviderMock = MockRepository.Create<ICostPriceProvider>();

            var loggerFactory = MockRepository
                .Create<ILoggerFactory>()
                .Setup(lf => lf.CreateLogger(nameof(ProductService)), LoggerMock.Object)
                .Object;

            storageMock = MockRepository
                .Create<IStorage>()
                .Setup(str => str.GetRepository<IProductRepository>(), productRepositoryMock.Object)
                .Setup(str => str.GetRepository<ICategoryRepository>(), categoryRepositoryMock.Object);

            productService = new ProductService(
                storageMock.Object,
                costPriceProviderMock.Object,
                loggerFactory);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName)]
        public async Task GetAllAsyncTest()
        {
            var products = new[] { CreateProduct() };

            productRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), products);

            var actual = await productService.GetAllAsync();

            var expected = CreateValidValidationResponse(products, ProductsRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName + "No products found")]
        public async Task GetAllAsyncNoSuppliesFoundTest()
        {
            productRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Product>());

            var actual = await productService.GetAllAsync();

            var expected = CreateValidValidationResponse(Enumerable.Empty<Product>(), NoProductsFoundMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncTest()
        {
            var product = CreateProduct();

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product);

            var actual = await productService.GetByIdAsync(ProductId);

            var expected = CreateValidValidationResponse(product, ProductRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName + "Product is not found")]
        public async Task GetByIdAsyncProductNotFoundTest()
        {
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await productService.GetByIdAsync(ProductId);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetCostPriceAsyncMethodName)]
        public async Task GetCostPriceAsyncTest()
        {
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), CreateProduct());

            costPriceProviderMock
                .SetupAsync(provider => provider.CalculateAsync(ProductId, 1, default(CancellationToken)), 25);

            var actual = await productService.GetCostPriceAsync(ProductId, 1);

            var expected = CreateValidValidationResponse(new ProductCostPrice(25), ProductCostPriceRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetCostPriceAsyncMethodName + "Product is not found")]
        public async Task GetCostPriceAsyncProductNotFoundTest()
        {
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await productService.GetCostPriceAsync(ProductId, 1);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName)]
        public async Task AddAsyncTest()
        {
            var product = CreateProduct();

            categoryRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), default(CancellationToken)), true);

            productRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Product, bool>>>(), default(CancellationToken)), false)
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Add(product), product);

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await productService.AddAsync(product);

            var expected = CreateValidValidationResponse(product, ProductAddedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "Category is not found")]
        public async Task AddAsyncCategoryNotFoundTest()
        {
            var product = CreateProduct();

            categoryRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), default(CancellationToken)), false);

            var actual = await productService.AddAsync(product);

            var expected = CreateWarningValidationResponse<Product>(CategoryNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "VendorCode is already exist")]
        public async Task AddAsyncVendorCodeAlreadyExistTest()
        {
            var product = CreateProduct();

            categoryRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), default(CancellationToken)), true);

            productRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Product, bool>>>(), default(CancellationToken)), true);

            var actual = await productService.AddAsync(product);

            var expected = CreateWarningValidationResponse<Product>(VendorExistMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName)]
        public async Task UpdateAsyncTest()
        {
            var product = CreateProduct();

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Product, bool>>>(), default(CancellationToken)), false)
                .Setup(repo => repo.Update(product), product);

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await productService.UpdateAsync(product);

            var expected = CreateValidValidationResponse(product, ProductUpdatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Product is not found")]
        public async Task UpdateAsyncProductNotFoundTest()
        {
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await productService.UpdateAsync(CreateProduct());

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "VendorCode duplication")]
        public async Task UpdateAsyncVendorCodeDuplicationTest()
        {
            var product = CreateProduct();

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Product, bool>>>(), default(CancellationToken)), true);
            
            var actual = await productService.UpdateAsync(product);

            var expected = CreateWarningValidationResponse<Product>(VendorExistMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        private static Product CreateProduct(int quantity = 10, string vendorCode = ProductVendorCode) =>
            new Product
            {
                Id = ProductId,
                CategoryId = CategoryId,
                VendorCode = vendorCode,
                Quantity = quantity
            };
    }
}
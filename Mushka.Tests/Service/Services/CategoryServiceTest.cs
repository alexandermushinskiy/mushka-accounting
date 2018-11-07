using System;
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
    public class CategoryServiceTest : ServiceTestBase
    {
        private const string TestCategoryName = Component.Service + nameof(CategoryService);
        private const string GetAllAsyncMethodName = nameof(CategoryService.GetAllAsync) + ". ";
        private const string GetByIdAsyncMethodName = nameof(CategoryService.GetByIdAsync) + ". ";
        private const string DeleteAsyncMethodName = nameof(CategoryService.DeleteAsync) + ". ";

        private const string CategoryName = "Category";
        private const string CategoriesRetrievedMessage = "Categories were successfully retrieved.";
        private const string NoCategoriesFoundMessage = "No categories found.";
        private static readonly Guid CategoryId = Guid.Parse("00000000000000000000000000000001");
        private static readonly string CategoryRetrievedMessage = $"Category with id {CategoryId} was successfully retrieved.";
        private static readonly string CategoryDeleteMessage = $"Category with id {CategoryId} was successfully deleted.";
        private static readonly string CategoryNotFoundMessage = $"Category with id {CategoryId} is not found.";
        private static readonly string CategoryHasProductsMessage = $"Category with id {CategoryId} contains products.";

        private readonly Mock<ICategoryRepository> categoryRepositoryMock;
        private readonly Mock<IProductRepository> productRepositoryMock;
        private readonly CategoryService categoryService;

        public CategoryServiceTest()
        {
            categoryRepositoryMock = MockRepository.Create<ICategoryRepository>();
            productRepositoryMock = MockRepository.Create<IProductRepository>();

            var loggerFactory = MockRepository
                .Create<ILoggerFactory>()
                .Setup(lf => lf.CreateLogger(nameof(CategoryService)), LoggerMock.Object)
                .Object;

            categoryService = new CategoryService(
                categoryRepositoryMock.Object,
                productRepositoryMock.Object,
                loggerFactory);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName)]
        public async Task GetAllAsyncTest()
        {
            var categories = new[] { CreateCategory() };

            categoryRepositoryMock.SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), categories);

            var actual = await categoryService.GetAllAsync();

            var expected = CreateValidValidationResponse(categories, CategoriesRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName + "No categories found")]
        public async Task GetAllAsyncNoCategoriesFoundTest()
        {
            categoryRepositoryMock.SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Category>());

            var actual = await categoryService.GetAllAsync();

            var expected = CreateValidValidationResponse(Enumerable.Empty<Category>(), NoCategoriesFoundMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncTest()
        {
            var category = CreateCategory();

            categoryRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), category);

            var actual = await categoryService.GetByIdAsync(CategoryId);

            var expected = CreateValidValidationResponse(category, CategoryRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncCategoryNotFoundTest()
        {
            categoryRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), null);

            var actual = await categoryService.GetByIdAsync(CategoryId);

            var expected = CreateWarningValidationResponse<Category>(CategoryNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName)]
        public async Task DeleteAsyncTest()
        {
            var category = CreateCategory();

            categoryRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), category)
                .SetupAsync(repo => repo.DeleteAsync(category, default(CancellationToken)), category);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByCategoryId(CategoryId, default(CancellationToken)), Enumerable.Empty<Product>());

            var actual = await categoryService.DeleteAsync(CategoryId);

            var expected = CreateValidValidationResponse(category, CategoryDeleteMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName)]
        public async Task DeleteAsyncCategoryNotFoundTest()
        {
            categoryRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), null);

            var actual = await categoryService.DeleteAsync(CategoryId);

            var expected = CreateWarningValidationResponse<Category>(CategoryNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(TestCategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Category has products")]
        public async Task DeleteAsyncCategoryHasProducts()
        {
            categoryRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), CreateCategory());

            productRepositoryMock
                .SetupAsync(repo => repo.GetByCategoryId(CategoryId, default(CancellationToken)), new[] { new Product() });

            var actual = await categoryService.DeleteAsync(CategoryId);

            var expected = CreateWarningValidationResponse<Category>(CategoryHasProductsMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        private static Category CreateCategory() =>
            new Category
            {
                Id = CategoryId,
                Name = CategoryName
            };
    }
}
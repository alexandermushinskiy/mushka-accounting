//using System;
//using System.ComponentModel;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading;
//using System.Threading.Tasks;
//using FluentAssertions;
//using Moq;
//using Mushka.Core.Extensibility.Logging;
//using Mushka.Core.Validation.Enums;
//using Mushka.Domain.Entities;
//using Mushka.Domain.Extensibility.Repositories;
//using Mushka.Service.Services;
//using Mushka.Tests.Common;
//using Xunit;

//namespace Mushka.Tests.Service.Services
//{
//    public class CategoryServiceTest : ServiceTestBase
//    {
//        private const string TestCategoryName = Component.Service + nameof(CategoryService);
//        private const string GetAllAsyncMethodName = nameof(CategoryService.GetAllAsync) + ". ";
//        private const string GetByIdAsyncMethodName = nameof(CategoryService.GetByIdAsync) + ". ";
//        private const string AddAsyncMethodName = nameof(CategoryService.AddAsync) + ". ";
//        private const string DeleteAsyncMethodName = nameof(CategoryService.DeleteAsync) + ". ";

//        private const string CategoryName = "Category";
//        private const string CategoriesRetrievedMessage = "Categories were successfully retrieved.";
//        private const string NoCategoriesFoundMessage = "No categories found.";
//        private static readonly Guid CategoryId = Guid.Parse("00000000000000000000000000000001");
//        private static readonly string CategoryRetrievedMessage = $"Category with id {CategoryId} was successfully retrieved.";
//        private static readonly string CategoryDeletedMessage = $"Category with id {CategoryId} was successfully deleted.";
//        private static readonly string CategoryNotFoundMessage = $"Category with id {CategoryId} is not found.";
//        private static readonly string CategoryHasProductsMessage = $"Category with id {CategoryId} contains products.";
//        private static readonly string CategoryNameDuplicationMessage = $"Category with name {CategoryName} is already exist.";
//        private static readonly string CategoryCreatedMessage = $"Category with id {CategoryId} was successfully created.";

//        private readonly Mock<IStorage> storageMock;
//        private readonly Mock<ICategoryRepository> categoryRepositoryMock;
//        private readonly Mock<IProductRepository> productRepositoryMock;
//        private readonly CategoryService categoryService;

//        public CategoryServiceTest()
//        {
//            categoryRepositoryMock = MockRepository.Create<ICategoryRepository>();
//            productRepositoryMock = MockRepository.Create<IProductRepository>();

//            var loggerFactory = MockRepository
//                .Create<ILoggerFactory>()
//                .Setup(lf => lf.CreateLogger(nameof(CategoryService)), LoggerMock.Object)
//                .Object;

//            storageMock = MockRepository
//                .Create<IStorage>()
//                .Setup(str => str.GetRepository<ICategoryRepository>(), categoryRepositoryMock.Object)
//                .Setup(str => str.GetRepository<IProductRepository>(), productRepositoryMock.Object);

//            categoryService = new CategoryService(storageMock.Object, loggerFactory);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = GetAllAsyncMethodName)]
//        public async Task GetAllAsyncTest()
//        {
//            var categories = new[] { CreateCategory() };

//            categoryRepositoryMock.SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), categories);

//            var actual = await categoryService.GetAllAsync();

//            var expected = CreateValidValidationResponse(categories, CategoriesRetrievedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = GetAllAsyncMethodName + "No categories found")]
//        public async Task GetAllAsyncNoCategoriesFoundTest()
//        {
//            categoryRepositoryMock.SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Category>());

//            var actual = await categoryService.GetAllAsync();

//            var expected = CreateValidValidationResponse(Enumerable.Empty<Category>(), NoCategoriesFoundMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = GetByIdAsyncMethodName)]
//        public async Task GetByIdAsyncTest()
//        {
//            var category = CreateCategory();

//            categoryRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), category);

//            var actual = await categoryService.GetByIdAsync(CategoryId);

//            var expected = CreateValidValidationResponse(category, CategoryRetrievedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = GetByIdAsyncMethodName)]
//        public async Task GetByIdAsyncCategoryNotFoundTest()
//        {
//            categoryRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), null);

//            var actual = await categoryService.GetByIdAsync(CategoryId);

//            var expected = CreateWarningValidationResponse<Category>(CategoryNotFoundMessage, ValidationStatusType.NotFound);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = AddAsyncMethodName)]
//        public async Task AddAsyncTest()
//        {
//            var category = CreateCategory();

//            categoryRepositoryMock
//                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), default(CancellationToken)), false)
//                .SetupAsync(repo => repo.AddAsync(category, default(CancellationToken)), category);

//            var actual = await categoryService.AddAsync(category);

//            var expected = CreateValidValidationResponse(category, CategoryCreatedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = AddAsyncMethodName + "Category name is duplicated")]
//        public async Task AddAsyncNameDuplicationTest()
//        {
//            var category = CreateCategory();

//            categoryRepositoryMock
//                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), default(CancellationToken)), true);

//            var actual = await categoryService.AddAsync(category);

//            var expected = CreateWarningValidationResponse<Category>(CategoryNameDuplicationMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = DeleteAsyncMethodName)]
//        public async Task DeleteAsyncTest()
//        {
//            var category = CreateCategory();

//            categoryRepositoryMock
//                .SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), category)
//                .SetupAsync(repo => repo.DeleteAsync(category, default(CancellationToken)), category);

//            productRepositoryMock
//                .SetupAsync(repo => repo.GetByCategoryId(CategoryId, default(CancellationToken)), Enumerable.Empty<Product>());

//            var actual = await categoryService.DeleteAsync(CategoryId);

//            var expected = CreateValidValidationResponse(category, CategoryDeletedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = DeleteAsyncMethodName)]
//        public async Task DeleteAsyncCategoryNotFoundTest()
//        {
//            categoryRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), null);

//            var actual = await categoryService.DeleteAsync(CategoryId);

//            var expected = CreateWarningValidationResponse<Category>(CategoryNotFoundMessage, ValidationStatusType.NotFound);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(TestCategoryName)]
//        [Fact(DisplayName = DeleteAsyncMethodName + "Category has products")]
//        public async Task DeleteAsyncCategoryHasProducts()
//        {
//            categoryRepositoryMock
//                .SetupAsync(repo => repo.GetByIdAsync(CategoryId, default(CancellationToken)), CreateCategory());

//            productRepositoryMock
//                .SetupAsync(repo => repo.GetByCategoryId(CategoryId, default(CancellationToken)), new[] { new Product() });

//            var actual = await categoryService.DeleteAsync(CategoryId);

//            var expected = CreateWarningValidationResponse<Category>(CategoryHasProductsMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        private static Category CreateCategory() =>
//            new Category
//            {
//                Id = CategoryId,
//                Name = CategoryName
//            };
//    }
//}
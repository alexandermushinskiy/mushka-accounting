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
//    public class SupplierServiceTest : ServiceTestBase
//    {
//        private const string CategoryName = Component.Service + nameof(SupplierService);
//        private const string GetAllAsyncMethodName = nameof(SupplierService.GetAllAsync) + ". ";
//        private const string GetByIdAsyncMethodName = nameof(SupplierService.GetByIdAsync) + ". ";
//        private const string AddAsyncMethodName = nameof(SupplierService.AddAsync) + ". ";
//        private const string UpdateAsyncMethodName = nameof(SupplierService.UpdateAsync) + ". ";
//        private const string DeleteAsyncMethodName = nameof(SupplierService.DeleteAsync) + ". ";

//        private const string SupplierName = "Company LTD";
//        private const string SuppliersRetrievedMessage = "Suppliers were successfully retrieved.";
//        private const string NoSuppliersFoundMessage = "No suppliers found.";
//        private static readonly Guid SupplierId = Guid.Parse("00000000000000000000000000000001");
//        private static readonly string SupplierNotFoundMessage = $"Supplier with id {SupplierId} is not found.";
//        private static readonly string SupplierRetrievedMessage = $"Supplier with id {SupplierId} was successfully retrieved.";
//        private static readonly string SupplierDeletedMessage = $"Supplier with id {SupplierId} was successfully deleted.";
//        private static readonly string SupplierCreatedMessage = $"Supplier with id {SupplierId} was successfully created.";
//        private static readonly string SupplierUpdatedMessage = $"Supplier with id {SupplierId} was successfully updated.";
//        private static readonly string SupplierNameDuplicationMessage = $"Supplier with name {SupplierName} is already exist.";

//        private readonly Mock<IStorage> storageMock;
//        private readonly Mock<ISupplierRepository> supplierRepositoryMock;
//        private readonly SupplierService supplierService;

//        public SupplierServiceTest()
//        {
//            supplierRepositoryMock = MockRepository.Create<ISupplierRepository>();

//            var loggerFactory = MockRepository
//                .Create<ILoggerFactory>()
//                .Setup(lf => lf.CreateLogger(nameof(SupplierService)), LoggerMock.Object)
//                .Object;

//            storageMock = MockRepository
//                .Create<IStorage>()
//                .Setup(str => str.GetRepository<ISupplierRepository>(), supplierRepositoryMock.Object);
            
//            supplierService = new SupplierService(storageMock.Object, loggerFactory);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = GetAllAsyncMethodName)]
//        public async Task GetAllAsyncTest()
//        {
//            var suppliers = new[] { CreateSupplier() };

//            supplierRepositoryMock.SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), suppliers);

//            var actual = await supplierService.GetAllAsync();

//            var expected = CreateValidValidationResponse(suppliers, SuppliersRetrievedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = GetAllAsyncMethodName + "No suppliers found")]
//        public async Task GetAllAsyncNoSuppliersFoundTest()
//        {
//            supplierRepositoryMock.SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Supplier>());

//            var actual = await supplierService.GetAllAsync();

//            var expected = CreateValidValidationResponse(Enumerable.Empty<Category>(), NoSuppliersFoundMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = GetByIdAsyncMethodName)]
//        public async Task GetByIdAsyncTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), supplier);

//            var actual = await supplierService.GetByIdAsync(SupplierId);

//            var expected = CreateValidValidationResponse(supplier, SupplierRetrievedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = GetByIdAsyncMethodName)]
//        public async Task GetByIdAsyncSupplierNotFoundTest()
//        {
//            supplierRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), null);

//            var actual = await supplierService.GetByIdAsync(SupplierId);

//            var expected = CreateWarningValidationResponse<Supplier>(SupplierNotFoundMessage, ValidationStatusType.NotFound);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = AddAsyncMethodName)]
//        public async Task AddAsyncTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock
//                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default(CancellationToken)), false)
//                .SetupAsync(repo => repo.AddAsync(supplier, default(CancellationToken)), supplier);

//            var actual = await supplierService.AddAsync(supplier);

//            var expected = CreateValidValidationResponse(supplier, SupplierCreatedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = AddAsyncMethodName + "Supplier name is duplicated")]
//        public async Task AddAsyncNameDuplicationTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock
//                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default(CancellationToken)), true);

//            var actual = await supplierService.AddAsync(supplier);

//            var expected = CreateWarningValidationResponse<Supplier>(SupplierNameDuplicationMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = UpdateAsyncMethodName)]
//        public async Task UpdateAsyncTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock
//                .SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), supplier)
//                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default(CancellationToken)), false)
//                .SetupAsync(repo => repo.UpdateAsync(supplier, default(CancellationToken)), supplier);

//            var actual = await supplierService.UpdateAsync(supplier);

//            var expected = CreateValidValidationResponse(supplier, SupplierUpdatedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = UpdateAsyncMethodName + "Supplier is not found")]
//        public async Task UpdateAsyncSupplierNotFoundTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock
//                .SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), null);

//            var actual = await supplierService.UpdateAsync(supplier);

//            var expected = CreateWarningValidationResponse<Category>(SupplierNotFoundMessage, ValidationStatusType.NotFound);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = UpdateAsyncMethodName + "Supplier name is duplicated")]
//        public async Task UpdateAsyncNameDuplicationTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock
//                .SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), supplier)
//                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default(CancellationToken)), true);

//            var actual = await supplierService.UpdateAsync(supplier);

//            var expected = CreateWarningValidationResponse<Supplier>(SupplierNameDuplicationMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = DeleteAsyncMethodName)]
//        public async Task DeleteAsyncTest()
//        {
//            var supplier = CreateSupplier();

//            supplierRepositoryMock
//                .SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), supplier)
//                .SetupAsync(repo => repo.DeleteAsync(supplier, default(CancellationToken)), supplier);
            
//            var actual = await supplierService.DeleteAsync(SupplierId);

//            var expected = CreateValidValidationResponse(supplier, SupplierDeletedMessage);
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Category(CategoryName)]
//        [Fact(DisplayName = DeleteAsyncMethodName + "Supplier is not found")]
//        public async Task DeleteAsyncSupplierNotFoundTest()
//        {
//            supplierRepositoryMock.SetupAsync(repo => repo.GetByIdAsync(SupplierId, default(CancellationToken)), null);

//            var actual = await supplierService.DeleteAsync(SupplierId);

//            var expected = CreateWarningValidationResponse<Category>(SupplierNotFoundMessage, ValidationStatusType.NotFound);
//            actual.Should().BeEquivalentTo(expected);
//        }
        
//        private static Supplier CreateSupplier() =>
//            new Supplier
//            {
//                Id = SupplierId,
//                Name = SupplierName
//            };
//    }
//}
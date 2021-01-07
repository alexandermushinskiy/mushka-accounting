using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Providers;
using Mushka.Tests.Common;
using Xunit;

namespace Mushka.Tests.Service.Providers
{
    public class OrderCustomerProviderTest : ServiceTestBase
    {
        private const string CategoryName = Component.Service + nameof(OrderCustomerProvider);
        private const string GetCustomerForNewOrderAsyncMethodName = nameof(OrderCustomerProvider.GetCustomerForNewOrderAsync) + ". ";
        private const string GetCustomerForExistingOrderAsyncMethodName = nameof(OrderCustomerProvider.GetCustomerForExistingOrderAsync) + ". ";

        private const string CustomerPhone = "0987775533";
        private const string CustomerFirstName = "Peter";
        private const string CustomerLastName = "Pen";
        private const string OtherCustomerFirstName = "Will";
        private const string OtherCustomerLastName = "Smith";
        private const string OtherCustomerPhone = "0487777777";
        private static readonly Guid CustomerId = Guid.Parse("00000000000000000000000000000001");
        private static readonly Guid StoredCustomerId = Guid.Parse("00000000000000000000000000000022");
        private static readonly string NewCustomerAddedMessage = $"New customer {CustomerFirstName} {CustomerLastName} was added.";
        private static readonly string ExistingCustomerAddedMessage = $"Existing customer {CustomerFirstName} {CustomerLastName} was added.";
        private static readonly string PhoneUsedByOtherCustomerMessage = $"Phone number {CustomerPhone} is already used for the customer {OtherCustomerFirstName} {OtherCustomerLastName}.";
        private static readonly string OldCustomerWasUpdatedMessage = $"Old customer {CustomerFirstName} {CustomerLastName} was updated.";
        
        private readonly Mock<ICustomerRepository> customerRepositoryMock;
        private readonly Mock<IGuidProvider> guidProviderMock;
        private readonly OrderCustomerProvider provider;

        public OrderCustomerProviderTest()
        {
            customerRepositoryMock = MockRepository.Create<ICustomerRepository>();
            guidProviderMock = MockRepository.Create<IGuidProvider>();

            var loggerFactory = MockRepository
                .Create<ILoggerFactory>()
                .Setup(lf => lf.CreateLogger(nameof(OrderCustomerProvider)), LoggerMock.Object)
                .Object;

            var storage = MockRepository
                .Create<IStorage>()
                .Setup(str => str.GetRepository<ICustomerRepository>(), customerRepositoryMock.Object)
                .Object;

            provider = new OrderCustomerProvider(storage, guidProviderMock.Object, loggerFactory);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetCustomerForNewOrderAsyncMethodName + "Add new customer")]
        public async Task GetCustomerForNewOrderAsyncAddNewCustomerTest()
        {
            var customer = CreateCustomer();

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByPhoneAsync(CustomerPhone, default(CancellationToken)), null)
                .Setup(repo => repo.Add(customer), customer);

            var actual = await provider.GetCustomerForNewOrderAsync(customer);

            var expected = CreateValidValidationResponse(customer, NewCustomerAddedMessage);
            actual.Should().BeEquivalentTo(expected);
        }
        
        [Category(CategoryName)]
        [Fact(DisplayName = GetCustomerForNewOrderAsyncMethodName + "Add existing customer")]
        public async Task GetCustomerForNewOrderAsyncAddExistingCustomerTest()
        {
            var customer = CreateCustomer();

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByPhoneAsync(CustomerPhone, default(CancellationToken)), customer);

            var actual = await provider.GetCustomerForNewOrderAsync(customer);

            var expected = CreateValidValidationResponse(customer, ExistingCustomerAddedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetCustomerForNewOrderAsyncMethodName + "Phone number is already registered for other customer")]
        public async Task GetCustomerForNewOrderAsyncPhoneUsedOtherCustomerTest()
        {
            var customer = CreateCustomer();
            var otherCustomer = CreateCustomer(OtherCustomerFirstName, OtherCustomerLastName);

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByPhoneAsync(CustomerPhone, default(CancellationToken)), otherCustomer);

            var actual = await provider.GetCustomerForNewOrderAsync(customer);

            var expected = CreateWarningValidationResponse<Customer>(PhoneUsedByOtherCustomerMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetCustomerForExistingOrderAsyncMethodName + "Add new customer")]
        public async Task GetCustomerForExistingOrderAsyncAddNewCustomerTest()
        {
            var customer = CreateCustomer(Guid.Empty, CustomerFirstName, CustomerLastName, CustomerPhone);
            var storedCustomer = CreateCustomer(StoredCustomerId, OtherCustomerFirstName, OtherCustomerLastName, OtherCustomerPhone);
            storedCustomer.Orders = new List<Order>{ new Order(), new Order() };

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(StoredCustomerId, default(CancellationToken)), storedCustomer)
                .SetupAsync(repo => repo.GetByPhoneAsync(CustomerPhone, default(CancellationToken)), null)
                .Setup(repo => repo.Add(customer), customer);

            var actual = await provider.GetCustomerForExistingOrderAsync(StoredCustomerId, customer);

            var expected = CreateValidValidationResponse(customer, NewCustomerAddedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetCustomerForExistingOrderAsyncMethodName + "Existing customer is updated")]
        public async Task GetCustomerForExistingOrderAsyncExistingCustomerUpdatedTest()
        {
            var customer = CreateCustomer(CustomerId, CustomerFirstName, CustomerLastName, CustomerPhone);
            var storedCustomer = CreateCustomer(StoredCustomerId, OtherCustomerFirstName, OtherCustomerLastName, OtherCustomerPhone);
            storedCustomer.Orders = new List<Order> { new Order() };

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(StoredCustomerId, default(CancellationToken)), storedCustomer)
                .SetupAsync(repo => repo.GetByPhoneAsync(CustomerPhone, default(CancellationToken)), null);

            var actual = await provider.GetCustomerForExistingOrderAsync(StoredCustomerId, customer);

            var expectedCustomer = CreateCustomer(StoredCustomerId, CustomerFirstName, CustomerLastName, CustomerPhone);
            var expected = CreateValidValidationResponse(expectedCustomer, OldCustomerWasUpdatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        //[Category(CategoryName)]
        //[Fact(DisplayName = GetCustomerForExistingOrderAsyncMethodName + "Other existing customer is added")]
        //public async Task GetCustomerForExistingOrderAsyncOtherExistingCustomerAdded()
        //{
        //}
        
        private static Customer CreateCustomer(string firstName = CustomerFirstName, string lastName = CustomerLastName) =>
            CreateCustomer(CustomerId, firstName, lastName, CustomerPhone);

        private static Customer CreateCustomer(Guid id, string firstName, string lastName, string phone) =>
            new Customer
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Phone = phone,
                Orders = new List<Order>()
            };
    }
}
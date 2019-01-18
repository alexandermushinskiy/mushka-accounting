using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Services;
using Mushka.Tests.Common;
using Xunit;

namespace Mushka.Tests.Service.Services
{
    public class OrderServiceTest : ServiceTestBase
    {
        private const string CategoryName = Component.Service + nameof(OrderService);
        private const string GetAllAsyncMethodName = nameof(OrderService.GetAllAsync) + ". ";
        private const string GetByIdAsyncMethodName = nameof(OrderService.GetByIdAsync) + ". ";
        private const string AddAsyncMethodName = nameof(OrderService.AddAsync) + ". ";
        private const string UpdateAsyncMethodName = nameof(OrderService.UpdateAsync) + ". ";
        private const string DeleteAsyncMethodName = nameof(OrderService.DeleteAsync) + ". ";
        private const string IsNumberExistAsyncMethodName = nameof(OrderService.IsNumberExistAsync) + ". ";

        private const string OrdersRetrievedMessage = "Orders were successfully retrieved.";
        private const string NoOrdersFoundMessage = "No orders found.";
        private const string OrderNumber = "111111-2222";
        private static readonly Guid OrderId = Guid.Parse("00000000000000000000000000000001");
        private static readonly Guid CustomerId = Guid.Parse("00000000000000000000000000000002");
        private static readonly Guid ProductId = Guid.Parse("00000000000000000000000000000003");
        private static readonly string OrderRetrievedMessage = $"Order with id {OrderId} was successfully retrieved.";
        private static readonly string OrderDeletedMessage = $"Order with id {OrderId} was successfully deleted.";
        private static readonly string OrderNotFoundMessage = $"Order with id {OrderId} is not found.";
        private static readonly string ProductNotFoundMessage = $"Product with id {ProductId} is not found.";
        private static readonly string OrderCreatedMessage = $"Order with id {OrderId} was successfully added.";
        private static readonly string OrderUpdatedMessage = $"Order with id {OrderId} was successfully updated.";
        private static readonly string ProductNotEnoughMessage = $"Product with id {ProductId} is not enough in stock.";
        private static readonly string OrderNumberValidMessage = $"Order number {OrderNumber} is valid.";
        private static readonly string OrderNumberNotValidMessage = $"Order number {OrderNumber} is not valid.";

        private readonly Mock<IStorage> storageMock;
        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly Mock<IProductRepository> productRepositoryMock;
        private readonly Mock<ICustomerRepository> customerRepositoryMock;
        private readonly Mock<ICostPriceProvider> costPriceProviderMock;
        private readonly OrderService orderService;

        public OrderServiceTest()
        {
            costPriceProviderMock = MockRepository.Create<ICostPriceProvider>();
            productRepositoryMock = MockRepository.Create<IProductRepository>();
            customerRepositoryMock = MockRepository.Create<ICustomerRepository>();
            orderRepositoryMock = MockRepository.Create<IOrderRepository>();

            var loggerFactory = MockRepository
                .Create<ILoggerFactory>()
                .Setup(lf => lf.CreateLogger(nameof(OrderService)), LoggerMock.Object)
                .Object;

            storageMock = MockRepository
                .Create<IStorage>()
                .Setup(str => str.GetRepository<IProductRepository>(), productRepositoryMock.Object)
                .Setup(str => str.GetRepository<ICustomerRepository>(), customerRepositoryMock.Object)
                .Setup(str => str.GetRepository<IOrderRepository>(), orderRepositoryMock.Object);

            orderService = new OrderService(
                storageMock.Object,
                costPriceProviderMock.Object,
                loggerFactory);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName)]
        public async Task GetAllAsyncTest()
        {
            var orders = new[] { CreateOrder(new[] { CreateOrderProduct(10) }) };

            orderRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), orders);

            var actual = await orderService.GetAllAsync();

            var expected = CreateValidValidationResponse(orders, OrdersRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetAllAsyncMethodName + "No orders found")]
        public async Task GetAllAsyncNoOrdersFoundTest()
        {
            orderRepositoryMock
                .SetupAsync(repo => repo.GetAllAsync(default(CancellationToken)), Enumerable.Empty<Order>());

            var actual = await orderService.GetAllAsync();

            var expected = CreateValidValidationResponse(Enumerable.Empty<Order>(), NoOrdersFoundMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(10) });

            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), order);

            var actual = await orderService.GetByIdAsync(OrderId);

            var expected = CreateValidValidationResponse(order, OrderRetrievedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = GetByIdAsyncMethodName)]
        public async Task GetByIdAsyncCategoryNotFoundTest()
        {
            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), null);

            var actual = await orderService.GetByIdAsync(OrderId);

            var expected = CreateWarningValidationResponse<Category>(OrderNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName)]
        public async Task AddAsyncTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(10) });
            var product = CreateProduct(20);
            const int ExpectedQuantity = 10;

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByOrderDetails(order.Customer, default(CancellationToken)), CreateCustomer());

            orderRepositoryMock
                .Setup(repo => repo.Add(order), order);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await orderService.AddAsync(order);

            var expected = CreateValidValidationResponse(order, OrderCreatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "New customer")]
        public async Task AddAsyncNewCustomerTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(10) });
            var product = CreateProduct(20);
            const int ExpectedQuantity = 10;

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByOrderDetails(order.Customer, default(CancellationToken)), null)
                .Setup(repo => repo.Add(order.Customer), CreateCustomer());

            orderRepositoryMock
                .Setup(repo => repo.Add(order), order);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedQuantity));
            
            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await orderService.AddAsync(order);

            var expected = CreateValidValidationResponse(order, OrderCreatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "Order product is not found")]
        public async Task AddAsyncOrderProductNotFoundTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(5) });
            
            customerRepositoryMock
                .SetupAsync(repo => repo.GetByOrderDetails(order.Customer, default(CancellationToken)), CreateCustomer());
            
            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);
            
            var actual = await orderService.AddAsync(order);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = AddAsyncMethodName + "Products quantity is not enough")]
        public async Task AddAsyncProductsQuantityNotEnoughTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(10) });
            var product = CreateProduct(5);

            customerRepositoryMock
                .SetupAsync(repo => repo.GetByOrderDetails(order.Customer, default(CancellationToken)), CreateCustomer());

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product);

            var actual = await orderService.AddAsync(order);

            var expected = CreateWarningValidationResponse<Product>(ProductNotEnoughMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName)]
        public async Task UpdateAsyncTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(5) });
            var storedOrder = CreateOrder(new[] { CreateOrderProduct(10) });
            var updatedOrder = CreateOrder(new[] {CreateOrderProduct(25)});
            var product = CreateProduct(20);
            const int ExpectedProductQuantity = 25;

            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), storedOrder)
                .Setup(repo => repo.Update(It.Is<Order>(ord => ord.Id == OrderId)), updatedOrder);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity));
            
            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await orderService.UpdateAsync(order);

            var expected = CreateValidValidationResponse(updatedOrder, OrderUpdatedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Order is not found")]
        public async Task UpdateAsyncOrderNotFoundTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(5) });

            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), null);

            var actual = await orderService.UpdateAsync(order);

            var expected = CreateWarningValidationResponse<Order>(OrderNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = UpdateAsyncMethodName + "Order product is not found")]
        public async Task UpdateAsyncOrderProductNotFoundTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(5) });

            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), order);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await orderService.UpdateAsync(order);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName)]
        public async Task DeleteAsyncTest()
        {
            var storedOrder = CreateOrder(new[] { CreateOrderProduct(3) });
            var product = CreateProduct(7);
            const int ExpectedProductQuantity = 10;

            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), storedOrder)
                .Setup(repo => repo.Delete(storedOrder), storedOrder);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), product)
                .Setup(repo => repo.Update(It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity)),
                    It.Is<Product>(prod => prod.Id == ProductId && prod.Quantity == ExpectedProductQuantity));

            storageMock
                .Setup(s => s.SaveAsync(default(CancellationToken)), Task.CompletedTask);

            var actual = await orderService.DeleteAsync(OrderId);

            var expected = CreateValidValidationResponse(storedOrder, OrderDeletedMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Order is not found")]
        public async Task DeleteAsyncOrderNotFoundTest()
        {
            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), null);

            var actual = await orderService.DeleteAsync(OrderId);

            var expected = CreateWarningValidationResponse<Order>(OrderNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = DeleteAsyncMethodName + "Order product is not found")]
        public async Task DeleteAsyncOrderProductNotFoundTest()
        {
            var order = CreateOrder(new[] { CreateOrderProduct(5) });

            orderRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(OrderId, default(CancellationToken)), order);

            productRepositoryMock
                .SetupAsync(repo => repo.GetByIdAsync(ProductId, default(CancellationToken)), null);

            var actual = await orderService.DeleteAsync(OrderId);

            var expected = CreateWarningValidationResponse<Product>(ProductNotFoundMessage, ValidationStatusType.NotFound);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = IsNumberExistAsyncMethodName + "Valid number")]
        public async Task IsNumberExistTest()
        {
            orderRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Order, bool>>>(), default(CancellationToken)), false);

            var actual = await orderService.IsNumberExistAsync(OrderNumber);

            var expected = CreateValidValidationResponse(true, OrderNumberValidMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        [Category(CategoryName)]
        [Fact(DisplayName = IsNumberExistAsyncMethodName + "Invalid number")]
        public async Task IsNumberExistInvalidTest()
        {
            orderRepositoryMock
                .SetupAsync(repo => repo.IsExistAsync(It.IsAny<Expression<Func<Order, bool>>>(), default(CancellationToken)), true);

            var actual = await orderService.IsNumberExistAsync(OrderNumber);

            var expected = CreateValidValidationResponse(false, OrderNumberNotValidMessage);
            actual.Should().BeEquivalentTo(expected);
        }

        private static Order CreateOrder(ICollection<OrderProduct> orderProducts) =>
            new Order
            {
                Id = OrderId,
                CustomerId = CustomerId,
                Customer = CreateCustomer(),
                Products = orderProducts
            };

        private static Customer CreateCustomer() =>
            new Customer
            {
                Id  = CustomerId,
                FirstName = "Peter",
                LastName = "Pen"
            };

        private static OrderProduct CreateOrderProduct(int quantity) =>
            new OrderProduct
            {
                OrderId = OrderId,
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
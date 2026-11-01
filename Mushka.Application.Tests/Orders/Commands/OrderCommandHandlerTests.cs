using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Orders.Commands;
using Mushka.Application.Orders.Converters;
using Mushka.Application.Orders.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Orders.Commands;

public class OrderCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();
    private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
    private readonly Mock<ICustomerRepository> _customerRepository = new Mock<ICustomerRepository>();
    private readonly Mock<IOrderCustomerProvider> _customerProvider = new Mock<IOrderCustomerProvider>();
    private readonly Mock<ICreateOrderConverter> _createConverter = new Mock<ICreateOrderConverter>();
    private readonly Mock<IUpdateOrderConverter> _updateConverter = new Mock<IUpdateOrderConverter>();
    private readonly OrderCommandHandler _handler;

    public OrderCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IOrderRepository>()).Returns(_orderRepository.Object);
        _storage.Setup(storage => storage.GetRepository<IProductRepository>()).Returns(_productRepository.Object);
        _storage.Setup(storage => storage.GetRepository<ICustomerRepository>()).Returns(_customerRepository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new OrderCommandHandler(
            _storage.Object,
            _customerProvider.Object,
            _createConverter.Object,
            _updateConverter.Object);
    }

    [Fact]
    public async Task Create_WhenCustomerIsRejected_DoesNotChangeStock()
    {
        var command = CreateCommand();
        var order = OrderWith(Guid.NewGuid(), Guid.NewGuid(), 1);
        _createConverter.Setup(converter => converter.ToOrder(command)).Returns(order);
        _customerProvider
            .Setup(provider => provider.GetCustomerForNewOrderAsync(order.Customer, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BusinessRuleException(ValidationErrors.CustomerPhoneNumberExist));

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CustomerPhoneNumberExist);
        _productRepository.Verify(repository => repository.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var command = CreateCommand();
        var order = OrderWith(Guid.NewGuid(), productId, 2);
        _createConverter.Setup(converter => converter.ToOrder(command)).Returns(order);
        SetupNewCustomer(order, Guid.NewGuid());
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _orderRepository.Verify(repository => repository.Add(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenStockIsInsufficient_ThrowsBusinessRuleException()
    {
        var productId = Guid.NewGuid();
        var command = CreateCommand();
        var order = OrderWith(Guid.NewGuid(), productId, 6);
        _createConverter.Setup(converter => converter.ToOrder(command)).Returns(order);
        SetupNewCustomer(order, Guid.NewGuid());
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = productId, Quantity = 5 });

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotEnoughInStock);
        _orderRepository.Verify(repository => repository.Add(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Create_DecreasesStock_SetsCustomerId_AddsOrder_AndSaves()
    {
        var productId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var command = CreateCommand();
        var order = OrderWith(Guid.NewGuid(), productId, 4);
        var product = new Product { Id = productId, Quantity = 10 };
        _createConverter.Setup(converter => converter.ToOrder(command)).Returns(order);
        SetupNewCustomer(order, customerId);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        order.CustomerId.Should().Be(customerId);
        product.Quantity.Should().Be(6);
        _productRepository.Verify(repository => repository.Update(product), Times.Once);
        _orderRepository.Verify(repository => repository.Add(order), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenOrderIsMissing_ThrowsNotFoundException()
    {
        var command = UpdateCommand(Guid.NewGuid());
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.OrderNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var command = UpdateCommand(Guid.NewGuid());
        var stored = OrderWith(command.Id, productId, 1);
        var updated = OrderWith(command.Id, productId, 2);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToOrder(command)).Returns(updated);
        SetupExistingCustomer(stored.CustomerId, updated, stored.CustomerId);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _orderRepository.Verify(repository => repository.Update(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenQuantityChanges_AdjustsStockByDifference()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = UpdateCommand(orderId);
        var product = new Product { Id = productId, Quantity = 20 };
        var stored = OrderWith(orderId, productId, 10);
        var updated = OrderWith(orderId, productId, 4);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToOrder(command)).Returns(updated);
        SetupExistingCustomer(stored.CustomerId, updated, stored.CustomerId);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(26);
        _productRepository.Verify(repository => repository.Update(product), Times.Once);
        _orderRepository.Verify(repository => repository.Update(updated), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenQuantityIsUnchanged_DoesNotUpdateProduct()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = UpdateCommand(orderId);
        var product = new Product { Id = productId, Quantity = 10 };
        var stored = OrderWith(orderId, productId, 2);
        var updated = OrderWith(orderId, productId, 2);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToOrder(command)).Returns(updated);
        SetupExistingCustomer(stored.CustomerId, updated, stored.CustomerId);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(10);
        _productRepository.Verify(repository => repository.Update(It.IsAny<Product>()), Times.Never);
        _orderRepository.Verify(repository => repository.Update(updated), Times.Once);
    }

    [Fact]
    public async Task Update_WhenProductsAreReplaced_ReturnsRemovedStockAndTakesNewStock()
    {
        var orderId = Guid.NewGuid();
        var removedProductId = Guid.NewGuid();
        var addedProductId = Guid.NewGuid();
        var command = UpdateCommand(orderId);
        var removedProduct = new Product { Id = removedProductId, Quantity = 22 };
        var addedProduct = new Product { Id = addedProductId, Quantity = 33 };
        var stored = OrderWith(orderId, removedProductId, 7);
        var updated = OrderWith(orderId, addedProductId, 5);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToOrder(command)).Returns(updated);
        SetupExistingCustomer(stored.CustomerId, updated, stored.CustomerId);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(addedProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(addedProduct);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(removedProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(removedProduct);

        await _handler.Handle(command, CancellationToken.None);

        addedProduct.Quantity.Should().Be(28);
        removedProduct.Quantity.Should().Be(29);
        _orderRepository.Verify(repository => repository.Update(updated), Times.Once);
    }

    [Fact]
    public async Task Update_WhenCustomerChangesAndOldCustomerHasNoOrders_DeletesOldCustomer()
    {
        var orderId = Guid.NewGuid();
        var oldCustomerId = Guid.NewGuid();
        var newCustomerId = Guid.NewGuid();
        var command = UpdateCommand(orderId);
        var stored = OrderWith(orderId, Guid.NewGuid(), 1, oldCustomerId);
        stored.Products = new List<OrderProduct>();
        var updated = OrderWith(orderId, Guid.NewGuid(), 0, newCustomerId);
        updated.Products = new List<OrderProduct>();
        var oldCustomer = new Customer { Id = oldCustomerId, Orders = new List<Order>() };
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToOrder(command)).Returns(updated);
        SetupExistingCustomer(oldCustomerId, updated, newCustomerId);
        _customerRepository
            .Setup(repository => repository.GetByIdAsync(oldCustomerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(oldCustomer);

        await _handler.Handle(command, CancellationToken.None);

        _customerRepository.Verify(repository => repository.Delete(oldCustomer), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task Update_WhenCustomerStaysTheSame_DoesNotDeleteCustomer()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var command = UpdateCommand(orderId);
        var stored = OrderWith(orderId, Guid.NewGuid(), 1, customerId);
        stored.Products = new List<OrderProduct>();
        var updated = OrderWith(orderId, Guid.NewGuid(), 0, customerId);
        updated.Products = new List<OrderProduct>();
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToOrder(command)).Returns(updated);
        SetupExistingCustomer(customerId, updated, customerId);

        await _handler.Handle(command, CancellationToken.None);

        _customerRepository.Verify(repository => repository.Delete(It.IsAny<Customer>()), Times.Never);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenOrderIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteOrderCommand(Guid.NewGuid());
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.OrderNotFound);
        _orderRepository.Verify(repository => repository.Delete(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var order = OrderWith(Guid.NewGuid(), productId, 2);
        var command = new DeleteOrderCommand(order.Id);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _customerRepository
            .Setup(repository => repository.GetOrdersCountAsync(order.CustomerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _orderRepository.Verify(repository => repository.Delete(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Delete_RestoresStock_DeletesSoleCustomer_AndDeletesOrder()
    {
        var productId = Guid.NewGuid();
        var product = new Product { Id = productId, Quantity = 3 };
        var customer = new Customer { Id = Guid.NewGuid(), Phone = "123" };
        var order = OrderWith(Guid.NewGuid(), productId, 2);
        order.Customer = customer;
        order.CustomerId = customer.Id;
        var command = new DeleteOrderCommand(order.Id);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _customerRepository
            .Setup(repository => repository.GetByPhoneAsync(customer.Phone, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);
        _customerRepository
            .Setup(repository => repository.GetOrdersCountAsync(customer.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(5);
        _customerRepository.Verify(repository => repository.Delete(customer), Times.Once);
        _orderRepository.Verify(repository => repository.Delete(order), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenCustomerHasOtherOrders_KeepsCustomer()
    {
        var order = OrderWith(Guid.NewGuid(), Guid.NewGuid(), 1);
        order.Products = new List<OrderProduct>();
        var command = new DeleteOrderCommand(order.Id);
        _orderRepository
            .Setup(repository => repository.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);
        _customerRepository
            .Setup(repository => repository.GetOrdersCountAsync(order.CustomerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);

        await _handler.Handle(command, CancellationToken.None);

        _customerRepository.Verify(repository => repository.Delete(It.IsAny<Customer>()), Times.Never);
        _orderRepository.Verify(repository => repository.Delete(order), Times.Once);
    }

    private void SetupNewCustomer(Order order, Guid customerId)
    {
        _customerProvider
            .Setup(provider => provider.GetCustomerForNewOrderAsync(order.Customer, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { Id = customerId });
    }

    private void SetupExistingCustomer(Guid storedCustomerId, Order order, Guid resolvedCustomerId)
    {
        _customerProvider
            .Setup(provider => provider.GetCustomerForExistingOrderAsync(storedCustomerId, order.Customer, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer { Id = resolvedCustomerId });
    }

    private static CreateOrderCommand CreateCommand() =>
        new CreateOrderCommand(OrderData(), CustomerData(), new List<OrderProductData>());

    private static UpdateOrderCommand UpdateCommand(Guid id) =>
        new UpdateOrderCommand(id, OrderData(), CustomerData(), new List<OrderProductData>());

    private static OrderData OrderData() =>
        new OrderData("O-1", new DateTime(2024, 3, 1), 100, PaymentMethod.Cash, null, 20, "Kyivska", "Kyiv", false, null);

    private static OrderCustomerData CustomerData() =>
        new OrderCustomerData(null, "Ann", "Smith", "123", null);

    private static Order OrderWith(Guid orderId, Guid productId, int quantity, Guid? customerId = null)
    {
        var resolvedCustomerId = customerId ?? Guid.NewGuid();
        return new Order
        {
            Id = orderId,
            CustomerId = resolvedCustomerId,
            Customer = new Customer { Id = resolvedCustomerId, Phone = "123", FirstName = "Ann", LastName = "Smith" },
            Products = new List<OrderProduct>
            {
                new OrderProduct { OrderId = orderId, ProductId = productId, Quantity = quantity }
            }
        };
    }
}

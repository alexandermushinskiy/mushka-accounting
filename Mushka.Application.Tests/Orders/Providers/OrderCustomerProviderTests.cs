using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Orders.Providers;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Orders.Providers;

public class OrderCustomerProviderTests
{
    private readonly Guid _generatedId = Guid.Parse("00000000-0000-0000-0000-000000000031");
    private readonly Mock<ICustomerRepository> _repository = new Mock<ICustomerRepository>();
    private readonly OrderCustomerProvider _provider;

    public OrderCustomerProviderTests()
    {
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<ICustomerRepository>()).Returns(_repository.Object);
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_generatedId);
        _provider = new OrderCustomerProvider(storage.Object, guidProvider.Object);
    }

    [Fact]
    public async Task GetCustomerForNewOrder_WhenPhoneIsNew_AddsCustomerWithGeneratedId()
    {
        var customer = Customer("Ann", "Smith", "123");
        _repository
            .Setup(repository => repository.GetByPhoneAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer)null);

        var actual = await _provider.GetCustomerForNewOrderAsync(customer, CancellationToken.None);

        actual.Should().BeSameAs(customer);
        actual.Id.Should().Be(_generatedId);
        _repository.Verify(repository => repository.Add(customer), Times.Once);
    }

    [Fact]
    public async Task GetCustomerForNewOrder_WhenPhoneAndNamesMatch_ReturnsStoredCustomer()
    {
        var stored = Customer("Ann", "Smith", "123");
        stored.Id = Guid.NewGuid();
        var customer = Customer("Ann", "Smith", "123");
        _repository
            .Setup(repository => repository.GetByPhoneAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);

        var actual = await _provider.GetCustomerForNewOrderAsync(customer, CancellationToken.None);

        actual.Should().BeSameAs(stored);
        customer.Id.Should().Be(stored.Id);
        _repository.Verify(repository => repository.Add(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task GetCustomerForNewOrder_WhenPhoneMatchesAndNamesDiffer_ThrowsBusinessRuleException()
    {
        var stored = Customer("Ann", "Smith", "123");
        var customer = Customer("Bob", "Smith", "123");
        _repository
            .Setup(repository => repository.GetByPhoneAsync("123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);

        var exception = await Record.ExceptionAsync(() => _provider.GetCustomerForNewOrderAsync(customer, CancellationToken.None));

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CustomerPhoneNumberExist);
    }

    [Fact]
    public async Task GetCustomerForExistingOrder_WhenSameCustomerNameChangesAndHasOtherOrders_Throws()
    {
        var customerId = Guid.NewGuid();
        var stored = Customer("Ann", "Smith", "123");
        stored.Id = customerId;
        stored.Orders = new List<Order> { new Order(), new Order() };
        var input = Customer("Bob", "Smith", "123");
        _repository.Setup(repository => repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(stored);
        _repository.Setup(repository => repository.GetByPhoneAsync("123", It.IsAny<CancellationToken>())).ReturnsAsync(stored);

        var exception = await Record.ExceptionAsync(() =>
            _provider.GetCustomerForExistingOrderAsync(customerId, input, CancellationToken.None));

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CustomerPhoneNumberExist);
    }

    [Fact]
    public async Task GetCustomerForExistingOrder_WhenSameCustomerHasOneOrder_KeepsStoredId()
    {
        var customerId = Guid.NewGuid();
        var stored = Customer("Ann", "Smith", "123");
        stored.Id = customerId;
        stored.Orders = new List<Order> { new Order() };
        var input = Customer("Ann", "Jones", "123");
        _repository.Setup(repository => repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(stored);
        _repository.Setup(repository => repository.GetByPhoneAsync("123", It.IsAny<CancellationToken>())).ReturnsAsync(stored);

        var actual = await _provider.GetCustomerForExistingOrderAsync(customerId, input, CancellationToken.None);

        actual.Id.Should().Be(customerId);
        _repository.Verify(repository => repository.Add(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task GetCustomerForExistingOrder_WhenAnotherCustomerHasSameNames_UsesThatCustomer()
    {
        var storedId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var stored = Customer("Ann", "Smith", "111");
        stored.Id = storedId;
        stored.Orders = new List<Order> { new Order() };
        var other = Customer("Bob", "Stone", "123");
        other.Id = otherId;
        var input = Customer("Bob", "Stone", "123");
        _repository.Setup(repository => repository.GetByIdAsync(storedId, It.IsAny<CancellationToken>())).ReturnsAsync(stored);
        _repository.Setup(repository => repository.GetByPhoneAsync("123", It.IsAny<CancellationToken>())).ReturnsAsync(other);

        var actual = await _provider.GetCustomerForExistingOrderAsync(storedId, input, CancellationToken.None);

        actual.Id.Should().Be(otherId);
    }

    [Fact]
    public async Task GetCustomerForExistingOrder_WhenPhoneIsNewAndCustomerHasOneOrder_ReusesStoredId()
    {
        var customerId = Guid.NewGuid();
        var stored = Customer("Ann", "Smith", "111");
        stored.Id = customerId;
        stored.Orders = new List<Order> { new Order() };
        var input = Customer("Ann", "Smith", "999");
        _repository.Setup(repository => repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(stored);
        _repository.Setup(repository => repository.GetByPhoneAsync("999", It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null);

        var actual = await _provider.GetCustomerForExistingOrderAsync(customerId, input, CancellationToken.None);

        actual.Id.Should().Be(customerId);
        _repository.Verify(repository => repository.Add(It.IsAny<Customer>()), Times.Never);
    }

    [Fact]
    public async Task GetCustomerForExistingOrder_WhenPhoneIsNewAndCustomerHasOtherOrders_AddsNewCustomer()
    {
        var customerId = Guid.NewGuid();
        var stored = Customer("Ann", "Smith", "111");
        stored.Id = customerId;
        stored.Orders = new List<Order> { new Order(), new Order() };
        var input = Customer("Ann", "Smith", "999");
        _repository.Setup(repository => repository.GetByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(stored);
        _repository.Setup(repository => repository.GetByPhoneAsync("999", It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null);

        var actual = await _provider.GetCustomerForExistingOrderAsync(customerId, input, CancellationToken.None);

        actual.Id.Should().Be(_generatedId);
        _repository.Verify(repository => repository.Add(input), Times.Once);
    }

    private static Customer Customer(string firstName, string lastName, string phone) =>
        new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Phone = phone,
            Orders = new List<Order>()
        };
}

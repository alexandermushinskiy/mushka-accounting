using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.CorporateOrders.Commands;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Application.Exceptions;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.CorporateOrders.Commands;

public class CorporateOrderCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<ICorporateOrderRepository> _repository = new Mock<ICorporateOrderRepository>();
    private readonly Mock<ICreateCorporateOrderConverter> _createConverter = new Mock<ICreateCorporateOrderConverter>();
    private readonly Mock<IUpdateCorporateOrderConverter> _updateConverter = new Mock<IUpdateCorporateOrderConverter>();
    private readonly CorporateOrderCommandHandler _handler;

    public CorporateOrderCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<ICorporateOrderRepository>()).Returns(_repository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new CorporateOrderCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
    }

    [Fact]
    public async Task Create_AddsOrderAndSaves()
    {
        var command = CreateCommand();
        var order = new CorporateOrder { Number = "CO-1" };
        _createConverter.Setup(converter => converter.ToCorporateOrder(command)).Returns(order);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.Add(order), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_DeletesProducts_UpdatesOrder_AndSaves()
    {
        var command = UpdateCommand(Guid.NewGuid());
        var order = new CorporateOrder { Id = command.Id };
        _updateConverter.Setup(converter => converter.ToCorporateOrder(command)).Returns(order);
        var sequence = new MockSequence();
        _repository.InSequence(sequence).Setup(repository => repository.DeleteProducts(order.Id));
        _repository.InSequence(sequence).Setup(repository => repository.Update(order));

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.DeleteProducts(order.Id), Times.Once);
        _repository.Verify(repository => repository.Update(order), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenOrderIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteCorporateOrderCommand(Guid.NewGuid());
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((CorporateOrder)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CorporateOrderNotFound);
        _repository.Verify(repository => repository.Delete(It.IsAny<CorporateOrder>()), Times.Never);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DeletesOrderAndSaves()
    {
        var order = new CorporateOrder { Id = Guid.NewGuid() };
        var command = new DeleteCorporateOrderCommand(order.Id);
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.Delete(order), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static CreateCorporateOrderCommand CreateCommand() =>
        new CreateCorporateOrderCommand(OrderData(), new List<CorporateOrderProductData>());

    private static UpdateCorporateOrderCommand UpdateCommand(Guid id) =>
        new UpdateCorporateOrderCommand(id, OrderData(), new List<CorporateOrderProductData>());

    private static CorporateOrderData OrderData() =>
        new CorporateOrderData(
            new DateTime(2024, 3, 1),
            "CO-1",
            100,
            PaymentMethod.Cash,
            null,
            null,
            null,
            null,
            null,
            20,
            "Kyivska",
            "Kyiv",
            "Acme",
            "Ann",
            "123",
            null,
            null);
}

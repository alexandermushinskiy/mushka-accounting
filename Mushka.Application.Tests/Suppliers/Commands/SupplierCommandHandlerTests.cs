using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Suppliers.Commands;
using Mushka.Application.Suppliers.Converters;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Suppliers.Commands;

public class SupplierCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<ISupplierRepository> _repository = new Mock<ISupplierRepository>();
    private readonly Mock<ICreateSupplierConverter> _createConverter = new Mock<ICreateSupplierConverter>();
    private readonly Mock<IUpdateSupplierConverter> _updateConverter = new Mock<IUpdateSupplierConverter>();
    private readonly SupplierCommandHandler _handler;

    public SupplierCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<ISupplierRepository>()).Returns(_repository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new SupplierCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
    }

    [Fact]
    public async Task Create_WhenNameExists_ThrowsBusinessRuleException()
    {
        var command = CreateCommand("Acme");
        SetupNameExists(true);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.SupplierWithNameExist);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenNameIsFree_AddsSupplierAndSaves()
    {
        var command = CreateCommand("Acme");
        var supplier = new Supplier { Name = "Acme" };
        SetupNameExists(false);
        _createConverter.Setup(converter => converter.ToSupplier(command)).Returns(supplier);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.Add(supplier), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenSupplierIsMissing_ThrowsNotFoundException()
    {
        var command = UpdateCommand(Guid.NewGuid(), "Acme");
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.SupplierNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenNameBelongsToAnotherSupplier_ThrowsBusinessRuleException()
    {
        var command = UpdateCommand(Guid.NewGuid(), "Acme");
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Supplier { Id = command.Id });
        SetupNameExists(true);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.SupplierWithNameExist);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_PreservesCreatedOn_AndSaves()
    {
        var createdOn = new DateTime(2024, 1, 2);
        var command = UpdateCommand(Guid.NewGuid(), "Acme");
        var existing = new Supplier { Id = command.Id, CreatedOn = createdOn };
        var updated = new Supplier { Id = command.Id };
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        SetupNameExists(false);
        _updateConverter.Setup(converter => converter.ToSupplier(command)).Returns(updated);

        await _handler.Handle(command, CancellationToken.None);

        updated.CreatedOn.Should().Be(createdOn);
        _repository.Verify(repository => repository.Update(updated), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenSupplierIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteSupplierCommand(Guid.NewGuid());
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Supplier)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.SupplierNotFound);
        _repository.Verify(repository => repository.Delete(It.IsAny<Supplier>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DeletesSupplierAndSaves()
    {
        var supplier = new Supplier { Id = Guid.NewGuid() };
        var command = new DeleteSupplierCommand(supplier.Id);
        _repository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(supplier);

        await _handler.Handle(command, CancellationToken.None);

        _repository.Verify(repository => repository.Delete(supplier), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private void SetupNameExists(bool exists)
    {
        _repository
            .Setup(repository => repository.IsExistAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists);
    }

    private static CreateSupplierCommand CreateCommand(string name) =>
        new CreateSupplierCommand(
            new SupplierData(name, null, null, null, null, null),
            new List<ContactPersonData>(),
            new List<PaymentCardData>());

    private static UpdateSupplierCommand UpdateCommand(Guid id, string name) =>
        new UpdateSupplierCommand(
            id,
            new SupplierData(name, null, null, null, null, null),
            new List<ContactPersonData>(),
            new List<PaymentCardData>());
}

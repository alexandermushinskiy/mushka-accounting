using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Products.Commands;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Products.Commands;

public class ProductCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
    private readonly Mock<ICategoryRepository> _categoryRepository = new Mock<ICategoryRepository>();
    private readonly Mock<ICreateProductConverter> _createConverter = new Mock<ICreateProductConverter>();
    private readonly Mock<IUpdateProductConverter> _updateConverter = new Mock<IUpdateProductConverter>();
    private readonly ProductCommandHandler _handler;

    public ProductCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IProductRepository>()).Returns(_productRepository.Object);
        _storage.Setup(storage => storage.GetRepository<ICategoryRepository>()).Returns(_categoryRepository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new ProductCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
    }

    [Fact]
    public async Task Create_WhenCategoryIsMissing_ThrowsNotFoundException()
    {
        var command = CreateCommand("SK-1");
        SetupCategoryExists(false);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenVendorCodeExists_ThrowsBusinessRuleException()
    {
        var command = CreateCommand("SK-1");
        SetupCategoryExists(true);
        SetupVendorCodeExists(true);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductWithVendorCodeExist);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_AddsProductAndSaves()
    {
        var command = CreateCommand("SK-1");
        var product = new Product { VendorCode = "SK-1" };
        SetupCategoryExists(true);
        SetupVendorCodeExists(false);
        _createConverter.Setup(converter => converter.ToProduct(command)).Returns(product);

        await _handler.Handle(command, CancellationToken.None);

        _productRepository.Verify(repository => repository.Add(product), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var command = UpdateCommand(Guid.NewGuid(), "SK-1");
        _productRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenVendorCodeBelongsToAnotherProduct_ThrowsBusinessRuleException()
    {
        var command = UpdateCommand(Guid.NewGuid(), "SK-1");
        _productRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = command.Id, Quantity = 4 });
        SetupVendorCodeExists(true);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductWithVendorCodeExist);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_PreservesQuantity_AndSaves()
    {
        var command = UpdateCommand(Guid.NewGuid(), "SK-1");
        var updated = new Product { Id = command.Id, Quantity = 0 };
        _productRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = command.Id, Quantity = 7 });
        SetupVendorCodeExists(false);
        _updateConverter.Setup(converter => converter.ToProduct(command)).Returns(updated);

        await _handler.Handle(command, CancellationToken.None);

        updated.Quantity.Should().Be(7);
        _productRepository.Verify(repository => repository.Update(updated), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteProductCommand(Guid.NewGuid());
        _productRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _productRepository.Verify(repository => repository.Delete(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Delete_DeletesProductAndSaves()
    {
        var product = new Product { Id = Guid.NewGuid() };
        var command = new DeleteProductCommand(product.Id);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        _productRepository.Verify(repository => repository.Delete(product), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private void SetupCategoryExists(bool exists)
    {
        _categoryRepository
            .Setup(repository => repository.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists);
    }

    private void SetupVendorCodeExists(bool exists)
    {
        _productRepository
            .Setup(repository => repository.IsExistAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists);
    }

    private static CreateProductCommand CreateCommand(string vendorCode) =>
        new CreateProductCommand(new ProductData("Socks", vendorCode, 10, Guid.NewGuid(), null, false));

    private static UpdateProductCommand UpdateCommand(Guid id, string vendorCode) =>
        new UpdateProductCommand(id, new ProductData("Socks", vendorCode, 10, Guid.NewGuid(), null, false));
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Exhibitions.Commands;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Exhibitions.Commands;

public class ExhibitionCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<IExhibitionRepository> _exhibitionRepository = new Mock<IExhibitionRepository>();
    private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
    private readonly Mock<ICreateExhibitionConverter> _createConverter = new Mock<ICreateExhibitionConverter>();
    private readonly Mock<IUpdateExhibitionConverter> _updateConverter = new Mock<IUpdateExhibitionConverter>();
    private readonly ExhibitionCommandHandler _handler;

    public ExhibitionCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<IExhibitionRepository>()).Returns(_exhibitionRepository.Object);
        _storage.Setup(storage => storage.GetRepository<IProductRepository>()).Returns(_productRepository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new ExhibitionCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
    }

    [Fact]
    public async Task Create_WhenProductIsMissing_ThrowsExhibitionNotFound()
    {
        var productId = Guid.NewGuid();
        var command = CreateCommand(productId, 2);
        var exhibition = ExhibitionWith(Guid.NewGuid(), productId, 2);
        _createConverter.Setup(converter => converter.ToExhibition(command)).Returns(exhibition);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExhibitionNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenStockIsInsufficient_ThrowsBusinessRuleException()
    {
        var productId = Guid.NewGuid();
        var command = CreateCommand(productId, 6);
        var exhibition = ExhibitionWith(Guid.NewGuid(), productId, 6);
        _createConverter.Setup(converter => converter.ToExhibition(command)).Returns(exhibition);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Id = productId, Quantity = 5 });

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotEnoughInStock);
        _exhibitionRepository.Verify(repository => repository.Add(It.IsAny<Exhibition>()), Times.Never);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_DecreasesProductQuantity_AddsExhibition_AndSaves()
    {
        var productId = Guid.NewGuid();
        var command = CreateCommand(productId, 10);
        var exhibition = ExhibitionWith(Guid.NewGuid(), productId, 10);
        var product = new Product { Id = productId, Quantity = 20 };
        _createConverter.Setup(converter => converter.ToExhibition(command)).Returns(exhibition);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(10);
        _productRepository.Verify(repository => repository.Update(product), Times.Once);
        _exhibitionRepository.Verify(repository => repository.Add(exhibition), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenExhibitionIsMissing_ThrowsNotFoundException()
    {
        var command = UpdateCommand(Guid.NewGuid(), Guid.NewGuid(), 1);
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Exhibition)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExhibitionNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var command = UpdateCommand(Guid.NewGuid(), productId, 2);
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExhibitionWith(command.Id, productId, 1));
        _updateConverter.Setup(converter => converter.ToExhibition(command)).Returns(ExhibitionWith(command.Id, productId, 2));
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenQuantityChanges_AdjustsStockByDifference()
    {
        var exhibitionId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = UpdateCommand(exhibitionId, productId, 5);
        var product = new Product { Id = productId, Quantity = 20 };
        var updated = ExhibitionWith(exhibitionId, productId, 5);
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(exhibitionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExhibitionWith(exhibitionId, productId, 10));
        _updateConverter.Setup(converter => converter.ToExhibition(command)).Returns(updated);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(25);
        _productRepository.Verify(repository => repository.Update(product), Times.Once);
        _exhibitionRepository.Verify(repository => repository.Update(updated), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenQuantityIsUnchanged_DoesNotUpdateProduct()
    {
        var exhibitionId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var command = UpdateCommand(exhibitionId, productId, 2);
        var product = new Product { Id = productId, Quantity = 10 };
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(exhibitionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExhibitionWith(exhibitionId, productId, 2));
        _updateConverter.Setup(converter => converter.ToExhibition(command)).Returns(ExhibitionWith(exhibitionId, productId, 2));
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(10);
        _productRepository.Verify(repository => repository.Update(It.IsAny<Product>()), Times.Never);
        _exhibitionRepository.Verify(repository => repository.Update(It.IsAny<Exhibition>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenProductsAreReplaced_ReturnsRemovedStockAndTakesNewStock()
    {
        var exhibitionId = Guid.NewGuid();
        var keptOutProductId = Guid.NewGuid();
        var addedProductId = Guid.NewGuid();
        var command = UpdateCommand(exhibitionId, addedProductId, 5);
        var removedProduct = new Product { Id = keptOutProductId, Quantity = 22 };
        var addedProduct = new Product { Id = addedProductId, Quantity = 33 };
        var updated = ExhibitionWith(exhibitionId, addedProductId, 5);
        var stored = new Exhibition
        {
            Id = exhibitionId,
            Products = new List<ExhibitionProduct>
            {
                new ExhibitionProduct { ExhibitionId = exhibitionId, ProductId = keptOutProductId, Quantity = 7 }
            }
        };
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(exhibitionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(stored);
        _updateConverter.Setup(converter => converter.ToExhibition(command)).Returns(updated);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(addedProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(addedProduct);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(keptOutProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(removedProduct);

        await _handler.Handle(command, CancellationToken.None);

        addedProduct.Quantity.Should().Be(28);
        removedProduct.Quantity.Should().Be(29);
        _exhibitionRepository.Verify(repository => repository.Update(updated), Times.Once);
    }

    [Fact]
    public async Task Update_WhenRemovedProductIsMissing_ThrowsNotFoundException()
    {
        var exhibitionId = Guid.NewGuid();
        var removedProductId = Guid.NewGuid();
        var command = UpdateCommand(exhibitionId, Guid.NewGuid(), 1);
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(exhibitionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ExhibitionWith(exhibitionId, removedProductId, 2));
        _updateConverter.Setup(converter => converter.ToExhibition(command)).Returns(new Exhibition
        {
            Id = exhibitionId,
            Products = new List<ExhibitionProduct>()
        });
        _productRepository
            .Setup(repository => repository.GetByIdAsync(removedProductId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _exhibitionRepository.Verify(repository => repository.Update(It.IsAny<Exhibition>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenExhibitionIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteExhibitionCommand(Guid.NewGuid());
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Exhibition)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ExhibitionNotFound);
        _exhibitionRepository.Verify(repository => repository.Delete(It.IsAny<Exhibition>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenProductIsMissing_ThrowsNotFoundException()
    {
        var productId = Guid.NewGuid();
        var exhibition = ExhibitionWith(Guid.NewGuid(), productId, 2);
        var command = new DeleteExhibitionCommand(exhibition.Id);
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(exhibition.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exhibition);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
        _exhibitionRepository.Verify(repository => repository.Delete(It.IsAny<Exhibition>()), Times.Never);
    }

    [Fact]
    public async Task Delete_ReturnsProductQuantity_DeletesExhibition_AndSaves()
    {
        var productId = Guid.NewGuid();
        var exhibition = ExhibitionWith(Guid.NewGuid(), productId, 3);
        var product = new Product { Id = productId, Quantity = 7 };
        var command = new DeleteExhibitionCommand(exhibition.Id);
        _exhibitionRepository
            .Setup(repository => repository.GetByIdAsync(exhibition.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exhibition);
        _productRepository
            .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        await _handler.Handle(command, CancellationToken.None);

        product.Quantity.Should().Be(10);
        _productRepository.Verify(repository => repository.Update(product), Times.Once);
        _exhibitionRepository.Verify(repository => repository.Delete(exhibition), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private static CreateExhibitionCommand CreateCommand(Guid productId, int quantity) =>
        new CreateExhibitionCommand(SampleData(), new List<ExhibitionProductData> { new ExhibitionProductData(productId, quantity, 1, 1) });

    private static UpdateExhibitionCommand UpdateCommand(Guid id, Guid productId, int quantity) =>
        new UpdateExhibitionCommand(id, SampleData(), new List<ExhibitionProductData> { new ExhibitionProductData(productId, quantity, 1, 1) });

    private static ExhibitionData SampleData() =>
        new ExhibitionData("Fair", DateTime.UtcNow, DateTime.UtcNow, "Kyiv", 1, PaymentMethod.Cash, null, null, null, null, 1, null);

    private static Exhibition ExhibitionWith(Guid exhibitionId, Guid productId, int quantity) =>
        new Exhibition
        {
            Id = exhibitionId,
            Products = new List<ExhibitionProduct>
            {
                new ExhibitionProduct { ExhibitionId = exhibitionId, ProductId = productId, Quantity = quantity }
            }
        };
}

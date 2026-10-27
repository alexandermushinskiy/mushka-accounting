using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Exceptions;
using Mushka.Application.Supplies.Commands;
using Mushka.Application.Supplies.Converters;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Supplies.Commands
{
    public class SupplyCommandHandlerTests
    {
        private readonly Mock<IStorage> _storage = new Mock<IStorage>();
        private readonly Mock<ISupplyRepository> _supplyRepository = new Mock<ISupplyRepository>();
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<ICreateSupplyConverter> _createConverter = new Mock<ICreateSupplyConverter>();
        private readonly Mock<IUpdateSupplyConverter> _updateConverter = new Mock<IUpdateSupplyConverter>();
        private readonly SupplyCommandHandler _handler;

        public SupplyCommandHandlerTests()
        {
            _storage.Setup(storage => storage.GetRepository<ISupplyRepository>()).Returns(_supplyRepository.Object);
            _storage.Setup(storage => storage.GetRepository<IProductRepository>()).Returns(_productRepository.Object);
            _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _handler = new SupplyCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
        }

        [Fact]
        public async Task Create_WhenProductIsMissing_ThrowsNotFoundException()
        {
            var productId = Guid.NewGuid();
            var command = CreateCommand(productId, 2);
            var supply = SupplyWith(Guid.NewGuid(), productId, 2);
            _createConverter.Setup(converter => converter.ToSupply(command)).Returns(supply);
            _productRepository
                .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
            _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Create_IncreasesProductQuantity_AddsSupply_AndSaves()
        {
            var productId = Guid.NewGuid();
            var command = CreateCommand(productId, 3);
            var supply = SupplyWith(Guid.NewGuid(), productId, 3);
            var product = new Product { Id = productId, Quantity = 4 };
            _createConverter.Setup(converter => converter.ToSupply(command)).Returns(supply);
            _productRepository
                .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _handler.Handle(command, CancellationToken.None);

            product.Quantity.Should().Be(7);
            _productRepository.Verify(repository => repository.Update(product), Times.Once);
            _supplyRepository.Verify(repository => repository.Add(supply), Times.Once);
            _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_WhenSupplyIsMissing_ThrowsNotFoundException()
        {
            var command = UpdateCommand(Guid.NewGuid(), Guid.NewGuid(), 1);
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Supply)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.SupplyNotFound);
            _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Update_WhenProductIsMissing_ThrowsNotFoundException()
        {
            var productId = Guid.NewGuid();
            var command = UpdateCommand(Guid.NewGuid(), productId, 2);
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(SupplyWith(command.Id, productId, 1));
            _updateConverter.Setup(converter => converter.ToSupply(command)).Returns(SupplyWith(command.Id, productId, 2));
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
            var supplyId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = UpdateCommand(supplyId, productId, 5);
            var product = new Product { Id = productId, Quantity = 10 };
            var updated = SupplyWith(supplyId, productId, 5);
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(supplyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(SupplyWith(supplyId, productId, 2));
            _updateConverter.Setup(converter => converter.ToSupply(command)).Returns(updated);
            _productRepository
                .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _handler.Handle(command, CancellationToken.None);

            product.Quantity.Should().Be(13);
            _productRepository.Verify(repository => repository.Update(product), Times.Once);
            _supplyRepository.Verify(repository => repository.Update(updated), Times.Once);
            _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_WhenQuantityIsUnchanged_DoesNotUpdateProduct()
        {
            var supplyId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = UpdateCommand(supplyId, productId, 2);
            var product = new Product { Id = productId, Quantity = 10 };
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(supplyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(SupplyWith(supplyId, productId, 2));
            _updateConverter.Setup(converter => converter.ToSupply(command)).Returns(SupplyWith(supplyId, productId, 2));
            _productRepository
                .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _handler.Handle(command, CancellationToken.None);

            product.Quantity.Should().Be(10);
            _productRepository.Verify(repository => repository.Update(It.IsAny<Product>()), Times.Never);
            _supplyRepository.Verify(repository => repository.Update(It.IsAny<Supply>()), Times.Once);
        }

        [Fact]
        public async Task Update_WhenProductIsRemoved_DecreasesStockByStoredQuantity()
        {
            var supplyId = Guid.NewGuid();
            var keptProductId = Guid.NewGuid();
            var removedProductId = Guid.NewGuid();
            var command = UpdateCommand(supplyId, keptProductId, 1);
            var removedProduct = new Product { Id = removedProductId, Quantity = 8 };
            var updated = SupplyWith(supplyId, keptProductId, 1);
            var stored = new Supply
            {
                Id = supplyId,
                Products = new List<SupplyProduct>
                {
                    new SupplyProduct { SupplyId = supplyId, ProductId = keptProductId, Quantity = 1 },
                    new SupplyProduct { SupplyId = supplyId, ProductId = removedProductId, Quantity = 3 }
                }
            };
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(supplyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(stored);
            _updateConverter.Setup(converter => converter.ToSupply(command)).Returns(updated);
            _productRepository
                .Setup(repository => repository.GetByIdAsync(keptProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product { Id = keptProductId, Quantity = 1 });
            _productRepository
                .Setup(repository => repository.GetByIdAsync(removedProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(removedProduct);

            await _handler.Handle(command, CancellationToken.None);

            removedProduct.Quantity.Should().Be(5);
            _productRepository.Verify(repository => repository.Update(removedProduct), Times.Once);
        }

        [Fact]
        public async Task Update_WhenRemovedProductIsMissing_ThrowsNotFoundException()
        {
            var supplyId = Guid.NewGuid();
            var removedProductId = Guid.NewGuid();
            var command = UpdateCommand(supplyId, Guid.NewGuid(), 1);
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(supplyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(SupplyWith(supplyId, removedProductId, 2));
            _updateConverter.Setup(converter => converter.ToSupply(command)).Returns(new Supply
            {
                Id = supplyId,
                Products = new List<SupplyProduct>()
            });
            _productRepository
                .Setup(repository => repository.GetByIdAsync(removedProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
            _supplyRepository.Verify(repository => repository.Update(It.IsAny<Supply>()), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenSupplyIsMissing_ThrowsNotFoundException()
        {
            var command = new DeleteSupplyCommand(Guid.NewGuid());
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Supply)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.SupplyNotFound);
            _supplyRepository.Verify(repository => repository.Delete(It.IsAny<Supply>()), Times.Never);
        }

        [Fact]
        public async Task Delete_WhenProductIsMissing_ThrowsNotFoundException()
        {
            var productId = Guid.NewGuid();
            var supply = SupplyWith(Guid.NewGuid(), productId, 2);
            var command = new DeleteSupplyCommand(supply.Id);
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(supply.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(supply);
            _productRepository
                .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

            exception.Should().BeOfType<NotFoundException>()
                .Which.ErrorKey.Should().Be(ValidationErrors.ProductNotFound);
            _supplyRepository.Verify(repository => repository.Delete(It.IsAny<Supply>()), Times.Never);
        }

        [Fact]
        public async Task Delete_DecreasesProductQuantity_DeletesSupply_AndSaves()
        {
            var productId = Guid.NewGuid();
            var supply = SupplyWith(Guid.NewGuid(), productId, 4);
            var product = new Product { Id = productId, Quantity = 9 };
            var command = new DeleteSupplyCommand(supply.Id);
            _supplyRepository
                .Setup(repository => repository.GetByIdAsync(supply.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(supply);
            _productRepository
                .Setup(repository => repository.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            await _handler.Handle(command, CancellationToken.None);

            product.Quantity.Should().Be(5);
            _productRepository.Verify(repository => repository.Update(product), Times.Once);
            _supplyRepository.Verify(repository => repository.Delete(supply), Times.Once);
            _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        private static CreateSupplyCommand CreateCommand(Guid productId, int quantity) =>
            new CreateSupplyCommand(
                new SupplyCommandModel(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, "desc", null, null, 1, PaymentMethod.Cash, null, null, null, 1, null),
                new List<SupplyProductCommandModel> { new SupplyProductCommandModel(productId, quantity, 1, 1) });

        private static UpdateSupplyCommand UpdateCommand(Guid id, Guid productId, int quantity) =>
            new UpdateSupplyCommand(
                id,
                new SupplyCommandModel(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, "desc", null, null, 1, PaymentMethod.Cash, null, null, null, 1, null),
                new List<SupplyProductCommandModel> { new SupplyProductCommandModel(productId, quantity, 1, 1) });

        private static Supply SupplyWith(Guid supplyId, Guid productId, int quantity) =>
            new Supply
            {
                Id = supplyId,
                Products = new List<SupplyProduct>
                {
                    new SupplyProduct { SupplyId = supplyId, ProductId = productId, Quantity = quantity }
                }
            };
    }
}

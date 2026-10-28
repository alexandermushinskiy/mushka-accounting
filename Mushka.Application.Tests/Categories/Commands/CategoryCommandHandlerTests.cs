using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Categories.Commands;
using Mushka.Application.Categories.Converters;
using Mushka.Application.Exceptions;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Application.Tests.Categories.Commands;

public class CategoryCommandHandlerTests
{
    private readonly Mock<IStorage> _storage = new Mock<IStorage>();
    private readonly Mock<ICategoryRepository> _categoryRepository = new Mock<ICategoryRepository>();
    private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
    private readonly Mock<ICreateCategoryConverter> _createConverter = new Mock<ICreateCategoryConverter>();
    private readonly Mock<IUpdateCategoryConverter> _updateConverter = new Mock<IUpdateCategoryConverter>();
    private readonly CategoryCommandHandler _handler;

    public CategoryCommandHandlerTests()
    {
        _storage.Setup(storage => storage.GetRepository<ICategoryRepository>()).Returns(_categoryRepository.Object);
        _storage.Setup(storage => storage.GetRepository<IProductRepository>()).Returns(_productRepository.Object);
        _storage.Setup(storage => storage.SaveAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _handler = new CategoryCommandHandler(_storage.Object, _createConverter.Object, _updateConverter.Object);
    }

    [Fact]
    public async Task Create_WhenNameExists_ThrowsBusinessRuleException()
    {
        var command = CreateCommand("Socks");
        SetupNameExists(true);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryWithNameExist);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Create_WhenNameIsFree_AddsCategoryAndSaves()
    {
        var command = CreateCommand("Socks");
        var category = new Category { Name = "Socks" };
        SetupNameExists(false);
        _createConverter.Setup(converter => converter.ToCategory(command)).Returns(category);

        await _handler.Handle(command, CancellationToken.None);

        _categoryRepository.Verify(repository => repository.Add(category), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenCategoryIsMissing_ThrowsNotFoundException()
    {
        var command = UpdateCommand(Guid.NewGuid(), "Socks");
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryNotFound);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_WhenNameBelongsToAnotherCategory_ThrowsBusinessRuleException()
    {
        var command = UpdateCommand(Guid.NewGuid(), "Socks");
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = command.Id });
        SetupNameExists(true);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryWithNameExist);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Update_PreservesOrder_AndSaves()
    {
        var command = UpdateCommand(Guid.NewGuid(), "Socks");
        var existing = new Category { Id = command.Id, Order = 7 };
        var updated = new Category { Id = command.Id };
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);
        SetupNameExists(false);
        _updateConverter.Setup(converter => converter.ToCategory(command)).Returns(updated);

        await _handler.Handle(command, CancellationToken.None);

        updated.Order.Should().Be(7);
        _categoryRepository.Verify(repository => repository.Update(updated), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenCategoryIsMissing_ThrowsNotFoundException()
    {
        var command = new DeleteCategoryCommand(Guid.NewGuid());
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category)null);

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<NotFoundException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryNotFound);
        _categoryRepository.Verify(repository => repository.Delete(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenCategoryHasProducts_ThrowsBusinessRuleException()
    {
        var category = new Category { Id = Guid.NewGuid() };
        var command = new DeleteCategoryCommand(category.Id);
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _productRepository
            .Setup(repository => repository.GetByCategoryId(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product> { new Product() });

        var exception = await Record.ExceptionAsync(() => _handler.Handle(command, CancellationToken.None).AsTask());

        exception.Should().BeOfType<BusinessRuleException>()
            .Which.ErrorKey.Should().Be(ValidationErrors.CategoryCannotBeDeletedWithProducts);
        _categoryRepository.Verify(repository => repository.Delete(It.IsAny<Category>()), Times.Never);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenCategoryHasNoProducts_DeletesCategoryAndSaves()
    {
        var category = new Category { Id = Guid.NewGuid() };
        var command = new DeleteCategoryCommand(category.Id);
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _productRepository
            .Setup(repository => repository.GetByCategoryId(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        await _handler.Handle(command, CancellationToken.None);

        _categoryRepository.Verify(repository => repository.Delete(category), Times.Once);
        _storage.Verify(storage => storage.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private void SetupNameExists(bool exists)
    {
        _categoryRepository
            .Setup(repository => repository.IsExistAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists);
    }

    private static CreateCategoryCommand CreateCommand(string name) =>
        new CreateCategoryCommand(new CategoryData(name, true, false));

    private static UpdateCategoryCommand UpdateCommand(Guid id, string name) =>
        new UpdateCategoryCommand(id, new CategoryData(name, true, false));
}

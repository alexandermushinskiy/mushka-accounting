using System;
using FluentAssertions;
using Moq;
using Mushka.Application.Categories.Commands;
using Mushka.Application.Categories.Converters;
using Mushka.Core.Extensibility.Providers;
using Xunit;

namespace Mushka.Application.Tests.Categories.Converters;

public class CreateCategoryConverterTests
{
    private readonly Guid _categoryId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private readonly CreateCategoryConverter _converter;

    public CreateCategoryConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_categoryId);
        _converter = new CreateCategoryConverter(guidProvider.Object);
    }

    [Fact]
    public void ToCategory_MapsCommand_AndAssignsGeneratedId()
    {
        var command = new CreateCategoryCommand(new CategoryData("Socks", true, false));

        var category = _converter.ToCategory(command);

        category.Id.Should().Be(_categoryId);
        category.Name.Should().Be("Socks");
        category.IsSizeRequired.Should().BeTrue();
        category.IsAdditional.Should().BeFalse();
        category.Order.Should().Be(0);
    }
}

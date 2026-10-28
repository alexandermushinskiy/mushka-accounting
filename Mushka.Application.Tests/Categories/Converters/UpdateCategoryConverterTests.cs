using System;
using FluentAssertions;
using Mushka.Application.Categories.Commands;
using Mushka.Application.Categories.Converters;
using Xunit;

namespace Mushka.Application.Tests.Categories.Converters;

public class UpdateCategoryConverterTests
{
    private readonly UpdateCategoryConverter _converter = new UpdateCategoryConverter();

    [Fact]
    public void ToCategory_UsesCommandId_AndLeavesOrderUnset()
    {
        var id = Guid.NewGuid();
        var command = new UpdateCategoryCommand(id, new CategoryData("Socks", false, true));

        var category = _converter.ToCategory(command);

        category.Id.Should().Be(id);
        category.Name.Should().Be("Socks");
        category.IsSizeRequired.Should().BeFalse();
        category.IsAdditional.Should().BeTrue();
        category.Order.Should().Be(0);
    }
}

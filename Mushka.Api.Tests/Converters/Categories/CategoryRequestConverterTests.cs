using System;
using FluentAssertions;
using Mushka.Api.ClientModels.Category;
using Mushka.Api.Converters.Categories;
using Xunit;

namespace Mushka.Api.Tests.Converters.Categories;

public class CategoryRequestConverterTests
{
    private readonly CategoryRequestConverter _converter = new CategoryRequestConverter();

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var request = new CreateCategoryRequestModel
        {
            Name = "Socks",
            IsSizeRequired = true,
            IsAdditional = false
        };

        var command = _converter.ToCreateCommand(request);

        command.Category.Should().BeEquivalentTo(new { Name = "Socks", IsSizeRequired = true, IsAdditional = false });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var request = new UpdateCategoryRequestModel
        {
            Name = "Socks",
            IsSizeRequired = false,
            IsAdditional = true
        };

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Category.Name.Should().Be("Socks");
        command.Category.IsSizeRequired.Should().BeFalse();
        command.Category.IsAdditional.Should().BeTrue();
    }
}

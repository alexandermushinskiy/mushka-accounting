using System;
using FluentAssertions;
using Mushka.Api.ClientModels.Product;
using Mushka.Api.Converters.Products;
using Xunit;

namespace Mushka.Api.Tests.Converters.Products;

public class ProductRequestConverterTests
{
    private readonly ProductRequestConverter _converter = new ProductRequestConverter();

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var categoryId = Guid.NewGuid();
        var sizeId = Guid.NewGuid();
        var request = new CreateProductRequestModel
        {
            Name = "Socks",
            VendorCode = "SK-1",
            RecommendedPrice = 12,
            CategoryId = categoryId,
            SizeId = sizeId,
            IsArchival = true
        };

        var command = _converter.ToCreateCommand(request);

        command.Product.Should().BeEquivalentTo(new
        {
            Name = "Socks",
            VendorCode = "SK-1",
            RecommendedPrice = (decimal?)12,
            CategoryId = categoryId,
            SizeId = (Guid?)sizeId,
            IsArchival = true
        });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var request = new UpdateProductRequestModel
        {
            Name = "Socks",
            VendorCode = "SK-1",
            CategoryId = categoryId
        };

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Product.Name.Should().Be("Socks");
        command.Product.VendorCode.Should().Be("SK-1");
        command.Product.CategoryId.Should().Be(categoryId);
        command.Product.SizeId.Should().BeNull();
        command.Product.IsArchival.Should().BeFalse();
    }
}

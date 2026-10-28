using System;
using FluentAssertions;
using Moq;
using Mushka.Application.Products.Commands;
using Mushka.Application.Products.Converters;
using Mushka.Core.Extensibility.Providers;
using Xunit;

namespace Mushka.Application.Tests.Products.Converters;

public class UpdateProductConverterTests
{
    private readonly DateTime _createdOn = new DateTime(2024, 8, 1);
    private readonly UpdateProductConverter _converter;

    public UpdateProductConverterTests()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.Setup(provider => provider.GetNow()).Returns(_createdOn);
        _converter = new UpdateProductConverter(dateTimeProvider.Object);
    }

    [Fact]
    public void ToProduct_UsesCommandId_AndSetsCreatedOnToNow()
    {
        var productId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var command = new UpdateProductCommand(productId, new ProductData("Socks", "SK-1", null, categoryId, null, false));

        var product = _converter.ToProduct(command);

        product.Id.Should().Be(productId);
        product.Name.Should().Be("Socks");
        product.VendorCode.Should().Be("SK-1");
        product.RecommendedPrice.Should().BeNull();
        product.CategoryId.Should().Be(categoryId);
        product.SizeId.Should().BeNull();
        product.IsArchival.Should().BeFalse();
        product.CreatedOn.Should().Be(_createdOn);
    }
}

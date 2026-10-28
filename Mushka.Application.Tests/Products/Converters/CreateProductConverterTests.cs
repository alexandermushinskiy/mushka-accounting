using System;
using FluentAssertions;
using Moq;
using Mushka.Application.Products.Commands;
using Mushka.Application.Products.Converters;
using Mushka.Core.Extensibility.Providers;
using Xunit;

namespace Mushka.Application.Tests.Products.Converters;

public class CreateProductConverterTests
{
    private readonly Guid _productId = Guid.Parse("00000000-0000-0000-0000-000000000031");
    private readonly DateTime _createdOn = new DateTime(2024, 5, 6, 7, 8, 9);
    private readonly CreateProductConverter _converter;

    public CreateProductConverterTests()
    {
        var guidProvider = new Mock<IGuidProvider>();
        guidProvider.Setup(provider => provider.NewGuid()).Returns(_productId);
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.Setup(provider => provider.GetNow()).Returns(_createdOn);
        _converter = new CreateProductConverter(guidProvider.Object, dateTimeProvider.Object);
    }

    [Fact]
    public void ToProduct_MapsCommand_AndAssignsGeneratedIdAndCreatedOn()
    {
        var categoryId = Guid.NewGuid();
        var sizeId = Guid.NewGuid();
        var command = new CreateProductCommand(new ProductData("Socks", "SK-1", 12.5m, categoryId, sizeId, true));

        var product = _converter.ToProduct(command);

        product.Id.Should().Be(_productId);
        product.Name.Should().Be("Socks");
        product.VendorCode.Should().Be("SK-1");
        product.RecommendedPrice.Should().Be(12.5m);
        product.CategoryId.Should().Be(categoryId);
        product.SizeId.Should().Be(sizeId);
        product.IsArchival.Should().BeTrue();
        product.CreatedOn.Should().Be(_createdOn);
        product.Quantity.Should().Be(0);
    }
}

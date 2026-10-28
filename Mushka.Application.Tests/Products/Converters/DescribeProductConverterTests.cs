using System;
using FluentAssertions;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Products.Converters;

public class DescribeProductConverterTests
{
    private readonly DescribeProductConverter _converter = new DescribeProductConverter();

    [Fact]
    public void ToReadModel_MapsProductCategoryAndSize()
    {
        var productId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var sizeId = Guid.NewGuid();
        var createdOn = new DateTime(2024, 1, 2);
        var product = new Product
        {
            Id = productId,
            Name = "Socks",
            VendorCode = "SK-1",
            RecommendedPrice = 15,
            CreatedOn = createdOn,
            CategoryId = categoryId,
            IsArchival = true,
            Category = new Category { Id = categoryId, Name = "Clothes" },
            Size = new Size { Id = sizeId, Name = "M" }
        };

        var readModel = _converter.ToReadModel(product);

        readModel.Id.Should().Be(productId);
        readModel.Name.Should().Be("Socks");
        readModel.VendorCode.Should().Be("SK-1");
        readModel.RecommendedPrice.Should().Be(15);
        readModel.CreatedOn.Should().Be(createdOn);
        readModel.CategoryId.Should().Be(categoryId);
        readModel.IsArchival.Should().BeTrue();
        readModel.Category.Id.Should().Be(categoryId);
        readModel.Category.Name.Should().Be("Clothes");
        readModel.Size.Id.Should().Be(sizeId);
        readModel.Size.Name.Should().Be("M");
    }

    [Fact]
    public void ToReadModel_WhenCategoryAndSizeAreMissing_LeavesThemNull()
    {
        var readModel = _converter.ToReadModel(new Product { Name = "Socks" });

        readModel.Category.Should().BeNull();
        readModel.Size.Should().BeNull();
    }
}

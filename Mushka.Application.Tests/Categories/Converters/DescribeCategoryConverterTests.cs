using System;
using FluentAssertions;
using Mushka.Application.Categories.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Categories.Converters;

public class DescribeCategoryConverterTests
{
    private readonly DescribeCategoryConverter _converter = new DescribeCategoryConverter();

    [Fact]
    public void ToReadModel_MapsCategory()
    {
        var categoryId = Guid.NewGuid();
        var category = new Category
        {
            Id = categoryId,
            Name = "Socks",
            Order = 4,
            IsSizeRequired = true,
            IsAdditional = true
        };

        var readModel = _converter.ToReadModel(category);

        readModel.Id.Should().Be(categoryId);
        readModel.Name.Should().Be("Socks");
        readModel.Order.Should().Be(4);
        readModel.IsSizeRequired.Should().BeTrue();
        readModel.IsAdditional.Should().BeTrue();
    }
}

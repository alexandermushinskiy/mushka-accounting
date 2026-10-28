using System;
using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Products.Converters;

public class GetProductSizesConverterTests
{
    private readonly GetProductSizesConverter _converter = new GetProductSizesConverter();

    [Fact]
    public void ToReadModel_MapsSizes()
    {
        var sizeId = Guid.NewGuid();
        var sizes = new List<Size> { new Size { Id = sizeId, Name = "M" } };

        var item = _converter.ToReadModel(sizes).Items.Should().ContainSingle().Subject;

        item.Id.Should().Be(sizeId);
        item.Name.Should().Be("M");
    }
}

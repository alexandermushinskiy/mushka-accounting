using System.Collections.Generic;
using FluentAssertions;
using Mushka.Application.Categories.Converters;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Application.Tests.Categories.Converters;

public class SearchCategoriesConverterTests
{
    private readonly SearchCategoriesConverter _converter = new SearchCategoriesConverter();

    [Fact]
    public void ToReadModel_MapsCategoriesAndTotal()
    {
        var categoryId = System.Guid.NewGuid();
        var categories = new List<Category>
        {
            new Category
            {
                Id = categoryId,
                Name = "Socks",
                Order = 3,
                IsSizeRequired = true,
                IsAdditional = false
            }
        };

        var readModel = _converter.ToReadModel(categories);

        readModel.Total.Should().Be(1);
        readModel.Items.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                Id = categoryId,
                Name = "Socks",
                Order = 3,
                IsSizeRequired = true,
                IsAdditional = false
            });
    }
}

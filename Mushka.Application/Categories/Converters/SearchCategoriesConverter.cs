using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Categories.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public sealed class SearchCategoriesConverter : ISearchCategoriesConverter
{
    public SearchCategoriesReadModel ToReadModel(IEnumerable<Category> categories)
    {
        var items = categories.Select(ToItem).ToList();

        return new SearchCategoriesReadModel
        {
            Total = items.Count,
            Items = items
        };
    }

    private static SearchCategoryReadModel ToItem(Category category) =>
        new SearchCategoryReadModel
        {
            Id = category.Id,
            Name = category.Name,
            Order = category.Order,
            IsSizeRequired = category.IsSizeRequired,
            IsAdditional = category.IsAdditional
        };
}

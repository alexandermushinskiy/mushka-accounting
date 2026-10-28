using System.Collections.Generic;
using Mushka.Application.Categories.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public interface ISearchCategoriesConverter
{
    SearchCategoriesReadModel ToReadModel(IEnumerable<Category> categories);
}

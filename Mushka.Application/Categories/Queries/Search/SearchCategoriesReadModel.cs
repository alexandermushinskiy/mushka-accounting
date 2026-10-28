using System;
using System.Collections.Generic;

namespace Mushka.Application.Categories.Queries.Search;

public sealed class SearchCategoriesReadModel
{
    public int Total { get; set; }

    public IEnumerable<SearchCategoryReadModel> Items { get; set; }
}

public sealed class SearchCategoryReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Order { get; set; }

    public bool IsSizeRequired { get; set; }

    public bool IsAdditional { get; set; }
}

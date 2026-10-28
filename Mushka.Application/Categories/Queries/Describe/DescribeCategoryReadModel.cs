using System;

namespace Mushka.Application.Categories.Queries.Describe;

public sealed class DescribeCategoryReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Order { get; set; }

    public bool IsSizeRequired { get; set; }

    public bool IsAdditional { get; set; }
}

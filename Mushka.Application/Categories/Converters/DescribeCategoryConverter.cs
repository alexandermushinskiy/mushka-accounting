using Mushka.Application.Categories.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public sealed class DescribeCategoryConverter : IDescribeCategoryConverter
{
    public DescribeCategoryReadModel ToReadModel(Category category) =>
        new DescribeCategoryReadModel
        {
            Id = category.Id,
            Name = category.Name,
            Order = category.Order,
            IsSizeRequired = category.IsSizeRequired,
            IsAdditional = category.IsAdditional
        };
}

using Mushka.Application.Categories.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public interface IDescribeCategoryConverter
{
    DescribeCategoryReadModel ToReadModel(Category category);
}

using Mushka.Application.Exhibitions.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public interface IDescribeExhibitionConverter
{
    DescribeExhibitionReadModel ToReadModel(Exhibition exhibition);
}

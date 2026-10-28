using Mushka.Application.Products.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public interface IDescribeProductConverter
{
    DescribeProductReadModel ToReadModel(Product product);
}

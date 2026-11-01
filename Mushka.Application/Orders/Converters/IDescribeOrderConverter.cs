using Mushka.Application.Orders.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Converters;

public interface IDescribeOrderConverter
{
    DescribeOrderReadModel ToReadModel(Order order);
}

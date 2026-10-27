using Mushka.Application.Supplies.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters
{
    public interface IDescribeSupplyConverter
    {
        DescribeSupplyReadModel ToReadModel(Supply supply);
    }
}

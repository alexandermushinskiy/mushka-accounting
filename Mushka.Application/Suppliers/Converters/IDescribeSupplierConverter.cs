using Mushka.Application.Suppliers.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters
{
    public interface IDescribeSupplierConverter
    {
        DescribeSupplierReadModel ToReadModel(Supplier supplier);
    }
}

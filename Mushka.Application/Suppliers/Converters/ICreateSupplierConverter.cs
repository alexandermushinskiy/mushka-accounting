using Mushka.Application.Suppliers.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Suppliers.Converters
{
    public interface ICreateSupplierConverter
    {
        Supplier ToSupplier(CreateSupplierCommand command);
    }
}

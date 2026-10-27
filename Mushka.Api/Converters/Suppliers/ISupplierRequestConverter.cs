using System;
using Mushka.Api.ClientModels.Supplier;
using Mushka.Application.Suppliers.Commands;

namespace Mushka.Api.Converters.Suppliers
{
    public interface ISupplierRequestConverter
    {
        CreateSupplierCommand ToCreateCommand(CreateSupplierRequestModel request);

        UpdateSupplierCommand ToUpdateCommand(Guid id, UpdateSupplierRequestModel request);
    }
}

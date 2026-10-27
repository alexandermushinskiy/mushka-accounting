using System;
using Mushka.Api.ClientModels.Supply;
using Mushka.Application.Supplies.Commands;

namespace Mushka.Api.Converters.Supplies
{
    public interface ISupplyRequestConverter
    {
        CreateSupplyCommand ToCreateCommand(SupplyRequestModel request);

        UpdateSupplyCommand ToUpdateCommand(Guid id, SupplyRequestModel request);
    }
}

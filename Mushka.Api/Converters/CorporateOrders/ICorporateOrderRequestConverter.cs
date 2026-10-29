using System;
using Mushka.Api.ClientModels.CorporateOrder;
using Mushka.Application.CorporateOrders.Commands;

namespace Mushka.Api.Converters.CorporateOrders;

public interface ICorporateOrderRequestConverter
{
    CreateCorporateOrderCommand ToCreateCommand(CorporateOrderRequestModel request);

    UpdateCorporateOrderCommand ToUpdateCommand(Guid id, CorporateOrderRequestModel request);
}

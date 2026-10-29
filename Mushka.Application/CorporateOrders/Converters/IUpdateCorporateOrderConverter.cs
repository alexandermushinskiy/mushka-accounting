using Mushka.Application.CorporateOrders.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public interface IUpdateCorporateOrderConverter
{
    CorporateOrder ToCorporateOrder(UpdateCorporateOrderCommand command);
}

using Mushka.Application.CorporateOrders.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Converters;

public interface ICreateCorporateOrderConverter
{
    CorporateOrder ToCorporateOrder(CreateCorporateOrderCommand command);
}

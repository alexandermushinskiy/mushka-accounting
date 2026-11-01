using Mushka.Application.Orders.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Converters;

public interface IUpdateOrderConverter
{
    Order ToOrder(UpdateOrderCommand command);
}

using Mushka.Application.Supplies.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters;

public interface ICreateSupplyConverter
{
    Supply ToSupply(CreateSupplyCommand command);
}

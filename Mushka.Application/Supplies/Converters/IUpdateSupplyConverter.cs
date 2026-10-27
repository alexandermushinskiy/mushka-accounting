using Mushka.Application.Supplies.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Converters;

public interface IUpdateSupplyConverter
{
    Supply ToSupply(UpdateSupplyCommand command);
}

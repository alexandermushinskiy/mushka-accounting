using Mushka.Application.Exhibitions.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public interface IUpdateExhibitionConverter
{
    Exhibition ToExhibition(UpdateExhibitionCommand command);
}

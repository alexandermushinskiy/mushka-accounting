using Mushka.Application.Exhibitions.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public interface ICreateExhibitionConverter
{
    Exhibition ToExhibition(CreateExhibitionCommand command);
}

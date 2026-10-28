using Mushka.Application.Categories.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public interface ICreateCategoryConverter
{
    Category ToCategory(CreateCategoryCommand command);
}

using Mushka.Application.Categories.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public interface IUpdateCategoryConverter
{
    Category ToCategory(UpdateCategoryCommand command);
}

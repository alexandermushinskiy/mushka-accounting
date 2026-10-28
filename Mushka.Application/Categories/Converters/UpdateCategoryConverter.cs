using Mushka.Application.Categories.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public sealed class UpdateCategoryConverter : IUpdateCategoryConverter
{
    public Category ToCategory(UpdateCategoryCommand command) =>
        new Category
        {
            Id = command.Id,
            Name = command.Category.Name,
            IsSizeRequired = command.Category.IsSizeRequired,
            IsAdditional = command.Category.IsAdditional
        };
}

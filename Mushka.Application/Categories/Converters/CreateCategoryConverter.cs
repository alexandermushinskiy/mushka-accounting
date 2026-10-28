using Mushka.Application.Categories.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Categories.Converters;

public sealed class CreateCategoryConverter : ICreateCategoryConverter
{
    private readonly IGuidProvider _guidProvider;

    public CreateCategoryConverter(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Category ToCategory(CreateCategoryCommand command) =>
        new Category
        {
            Id = _guidProvider.NewGuid(),
            Name = command.Category.Name,
            IsSizeRequired = command.Category.IsSizeRequired,
            IsAdditional = command.Category.IsAdditional
        };
}

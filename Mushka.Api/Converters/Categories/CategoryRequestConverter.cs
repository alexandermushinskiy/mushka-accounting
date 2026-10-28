using System;
using Mushka.Api.ClientModels.Category;
using Mushka.Application.Categories.Commands;

namespace Mushka.Api.Converters.Categories;

public sealed class CategoryRequestConverter : ICategoryRequestConverter
{
    public CreateCategoryCommand ToCreateCommand(CreateCategoryRequestModel request) =>
        new CreateCategoryCommand(ToCategory(request.Name, request.IsSizeRequired, request.IsAdditional));

    public UpdateCategoryCommand ToUpdateCommand(Guid id, UpdateCategoryRequestModel request) =>
        new UpdateCategoryCommand(id, ToCategory(request.Name, request.IsSizeRequired, request.IsAdditional));

    private static CategoryData ToCategory(string name, bool isSizeRequired, bool isAdditional) =>
        new CategoryData(name, isSizeRequired, isAdditional);
}

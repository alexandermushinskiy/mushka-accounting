using System;
using Mushka.Api.ClientModels.Category;
using Mushka.Application.Categories.Commands;

namespace Mushka.Api.Converters.Categories;

public interface ICategoryRequestConverter
{
    CreateCategoryCommand ToCreateCommand(CreateCategoryRequestModel request);

    UpdateCategoryCommand ToUpdateCommand(Guid id, UpdateCategoryRequestModel request);
}

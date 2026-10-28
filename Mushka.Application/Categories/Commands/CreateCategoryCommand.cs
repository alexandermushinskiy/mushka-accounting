using Mediator;

namespace Mushka.Application.Categories.Commands;

public sealed record CreateCategoryCommand(CategoryData Category) : ICommand;

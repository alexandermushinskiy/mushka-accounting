using System;
using Mediator;

namespace Mushka.Application.Categories.Commands;

public sealed record UpdateCategoryCommand(Guid Id, CategoryData Category) : ICommand;

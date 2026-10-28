using System;
using Mediator;

namespace Mushka.Application.Categories.Commands;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;

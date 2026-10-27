using System;
using Mediator;

namespace Mushka.Application.Exhibitions.Commands;

public sealed record DeleteExhibitionCommand(Guid Id) : ICommand;

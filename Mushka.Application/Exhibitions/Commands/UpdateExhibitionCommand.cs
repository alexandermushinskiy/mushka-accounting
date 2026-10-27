using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Exhibitions.Commands;

public sealed record UpdateExhibitionCommand(
    Guid Id,
    ExhibitionData Exhibition,
    IReadOnlyList<ExhibitionProductData> Products) : ICommand;

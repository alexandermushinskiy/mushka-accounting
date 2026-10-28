using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Products.Queries.Export;

public sealed record ExportProductsQuery(string Title, IReadOnlyList<Guid> ProductIds) : IQuery<ExportProductsReadModel>;

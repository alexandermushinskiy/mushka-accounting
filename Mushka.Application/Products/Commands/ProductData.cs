using System;

namespace Mushka.Application.Products.Commands;

public sealed record ProductData(
    string Name,
    string VendorCode,
    decimal? RecommendedPrice,
    Guid CategoryId,
    Guid? SizeId,
    bool IsArchival);

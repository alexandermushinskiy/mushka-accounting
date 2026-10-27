using System;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Commands;

public sealed record ExhibitionData(
    string Name,
    DateTime FromDate,
    DateTime ToDate,
    string City,
    decimal ParticipationCost,
    PaymentMethod ParticipationCostMethod,
    decimal? AccommodationCost,
    PaymentMethod? AccommodationCostMethod,
    decimal? FareCost,
    PaymentMethod? FareCostMethod,
    decimal Profit,
    string Notes);

public sealed record ExhibitionProductData(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal CostPrice);

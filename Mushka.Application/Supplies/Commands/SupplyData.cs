using System;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Commands;

public sealed record SupplyData(
    Guid SupplierId,
    DateTime RequestDate,
    DateTime ReceivedDate,
    string Description,
    decimal? Prepayment,
    PaymentMethod? PrepaymentMethod,
    decimal Cost,
    PaymentMethod CostMethod,
    decimal? DeliveryCost,
    PaymentMethod? DeliveryCostMethod,
    decimal? BankFee,
    decimal TotalCost,
    string Notes);

public sealed record SupplyProductData(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal CostPrice);

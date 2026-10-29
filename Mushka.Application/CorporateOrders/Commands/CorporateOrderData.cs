using System;
using Mushka.Domain.Entities;

namespace Mushka.Application.CorporateOrders.Commands;

public sealed record CorporateOrderData(
    DateTime CreatedOn,
    string OrderNumber,
    decimal Cost,
    PaymentMethod CostMethod,
    decimal? Prepayment,
    PaymentMethod? PrepaymentMethod,
    decimal? DeliveryCost,
    PaymentMethod? DeliveryCostMethod,
    int? Tax,
    decimal Profit,
    string Region,
    string City,
    string CompanyName,
    string ContactPerson,
    string Phone,
    string Email,
    string Notes);

public sealed record CorporateOrderProductData(
    string Name,
    int Quantity,
    decimal UnitPrice,
    decimal CostPrice);

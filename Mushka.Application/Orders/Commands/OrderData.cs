using System;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Commands;

public sealed record OrderData(
    string Number,
    DateTime OrderDate,
    decimal Cost,
    PaymentMethod CostMethod,
    int? Discount,
    decimal Profit,
    string Region,
    string City,
    bool IsWholesale,
    string Notes);

public sealed record OrderCustomerData(
    Guid? Id,
    string FirstName,
    string LastName,
    string Phone,
    string Email);

public sealed record OrderProductData(
    Guid ProductId,
    int Quantity,
    decimal UnitPrice,
    decimal CostPrice);

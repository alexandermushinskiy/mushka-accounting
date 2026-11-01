using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Orders.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Converters;

public sealed class UpdateOrderConverter : IUpdateOrderConverter
{
    private readonly IGuidProvider _guidProvider;

    public UpdateOrderConverter(IGuidProvider guidProvider)
    {
        _guidProvider = guidProvider;
    }

    public Order ToOrder(UpdateOrderCommand command)
    {
        var customerId = command.Customer.Id ?? _guidProvider.NewGuid();

        return new Order
        {
            Id = command.Id,
            OrderDate = command.Order.OrderDate,
            Number = command.Order.Number,
            Cost = command.Order.Cost,
            CostMethod = command.Order.CostMethod,
            Discount = command.Order.Discount == 0 ? null : command.Order.Discount,
            Profit = command.Order.Profit,
            IsWholesale = command.Order.IsWholesale,
            Notes = command.Order.Notes,
            Region = command.Order.Region,
            City = command.Order.City,
            Customer = ToCustomer(customerId, command.Customer),
            Products = ToProducts(command.Id, command.Products)
        };
    }

    private static Customer ToCustomer(Guid customerId, OrderCustomerData customer) =>
        new Customer
        {
            Id = customerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Phone = customer.Phone,
            Email = customer.Email
        };

    private static List<OrderProduct> ToProducts(Guid orderId, IEnumerable<OrderProductData> products) =>
        (products ?? Enumerable.Empty<OrderProductData>())
            .Select(product => new OrderProduct
            {
                OrderId = orderId,
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                CostPrice = product.CostPrice
            })
            .ToList();
}

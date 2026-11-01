using System.Linq;
using Mushka.Application.Orders.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Converters;

public sealed class DescribeOrderConverter : IDescribeOrderConverter
{
    public DescribeOrderReadModel ToReadModel(Order order) =>
        new DescribeOrderReadModel
        {
            Order = ToOrder(order),
            Customer = ToCustomer(order.Customer),
            Products = order.Products?.Select(ToProduct) ?? Enumerable.Empty<DescribeOrderProductReadModel>()
        };

    private static DescribeOrderDetailsReadModel ToOrder(Order order) =>
        new DescribeOrderDetailsReadModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Number = order.Number,
            Cost = order.Cost,
            CostMethod = order.CostMethod,
            Discount = order.Discount ?? 0,
            Profit = order.Profit,
            IsWholesale = order.IsWholesale,
            Notes = order.Notes,
            Region = order.Region,
            City = order.City
        };

    private static DescribeOrderCustomerReadModel ToCustomer(Customer customer) =>
        new DescribeOrderCustomerReadModel
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Phone = customer.Phone,
            Email = customer.Email
        };

    private static DescribeOrderProductReadModel ToProduct(OrderProduct orderProduct) =>
        new DescribeOrderProductReadModel
        {
            Id = orderProduct.ProductId,
            Name = orderProduct.Product?.Name,
            VendorCode = orderProduct.Product?.VendorCode,
            SizeName = orderProduct.Product?.Size?.Name,
            Quantity = orderProduct.Quantity,
            UnitPrice = orderProduct.UnitPrice,
            CostPrice = orderProduct.CostPrice
        };
}

using System.Linq;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;

namespace Mushka.Infrastructure.DataAccess.Converters
{
    internal static class DtoConverter
    {
        public static OrderSummaryDto ToOrderSummaryDto(Order order)
        {
            return new OrderSummaryDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.Number,
                Cost = order.Cost,
                Address = $"{order.Region}, {order.City}",
                CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}",
                ProductsCount = order.Products.Sum(p => p.Quantity),
                IsWholesale = order.IsWholesale
            };
        }
    }
}
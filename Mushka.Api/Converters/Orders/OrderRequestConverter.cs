using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Api.ClientModels.Order;
using Mushka.Application.Orders.Commands;
using Mushka.Application.Orders.Queries.Search;

namespace Mushka.Api.Converters.Orders;

public sealed class OrderRequestConverter : IOrderRequestConverter
{
    public SearchOrdersQuery ToSearchQuery(SearchOrdersRequestModel request) =>
        new SearchOrdersQuery(
            request.Query?.Customer?.Name?.Like,
            request.Query?.Order?.OrderDate?.From,
            request.Query?.Order?.OrderDate?.To,
            request.Navigation.Page.From,
            request.Navigation.Page.Size,
            request.Navigation.Sort.Key,
            request.Navigation.Sort.Order);

    public CreateOrderCommand ToCreateCommand(OrderRequestModel request) =>
        new CreateOrderCommand(ToOrder(request), ToCustomer(request.Customer), ToProducts(request.Products));

    public UpdateOrderCommand ToUpdateCommand(Guid id, OrderRequestModel request) =>
        new UpdateOrderCommand(id, ToOrder(request), ToCustomer(request.Customer), ToProducts(request.Products));

    private static OrderData ToOrder(OrderRequestModel request) =>
        new OrderData(
            request.Number,
            request.OrderDate,
            request.Cost,
            request.CostMethod,
            request.Discount,
            request.Profit,
            request.Region,
            request.City,
            request.IsWholesale,
            request.Notes);

    private static OrderCustomerData ToCustomer(OrderCustomerRequestModel customer) =>
        new OrderCustomerData(
            customer.Id,
            customer.FirstName,
            customer.LastName,
            customer.Phone,
            customer.Email);

    private static IReadOnlyList<OrderProductData> ToProducts(IEnumerable<OrderProductRequestModel> products) =>
        (products ?? Enumerable.Empty<OrderProductRequestModel>())
            .Select(product => new OrderProductData(
                product.ProductId,
                product.Quantity,
                product.UnitPrice,
                product.CostPrice))
            .ToList();
}

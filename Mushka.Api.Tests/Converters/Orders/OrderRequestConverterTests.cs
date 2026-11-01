using System;
using FluentAssertions;
using Mushka.Api.ClientModels.Order;
using Mushka.Api.Converters.Orders;
using Mushka.Domain.Entities;
using Mushka.Domain.Strings;
using Xunit;

namespace Mushka.Api.Tests.Converters.Orders;

public class OrderRequestConverterTests
{
    private readonly OrderRequestConverter _converter = new OrderRequestConverter();

    [Fact]
    public void ToSearchQuery_MapsFiltersAndNavigation()
    {
        var from = new DateTime(2024, 1, 1);
        var to = new DateTime(2024, 1, 31);
        var request = new SearchOrdersRequestModel
        {
            Query = new SearchOrdersQueryRequestModel
            {
                Customer = new SearchOrdersCustomerRequestModel
                {
                    Name = new SearchOrdersLikeRequestModel { Like = "Ann" }
                },
                Order = new SearchOrdersOrderRequestModel
                {
                    OrderDate = new SearchOrdersBetweenRequestModel { From = from, To = to }
                }
            },
            Navigation = new SearchOrdersNavigationRequestModel
            {
                Page = new SearchOrdersPageRequestModel { From = 1, Size = 20 },
                Sort = new SearchOrdersSortRequestModel { Key = "orderDate", Order = SortOrder.Asc }
            }
        };

        var query = _converter.ToSearchQuery(request);

        query.CustomerName.Should().Be("Ann");
        query.OrderDateFrom.Should().Be(from);
        query.OrderDateTo.Should().Be(to);
        query.CurrentPage.Should().Be(1);
        query.PageSize.Should().Be(20);
        query.SortKey.Should().Be("orderDate");
        query.SortOrder.Should().Be(SortOrder.Asc);
    }

    [Fact]
    public void ToSearchQuery_WhenQueryIsMissing_LeavesFiltersEmpty()
    {
        var request = new SearchOrdersRequestModel
        {
            Navigation = new SearchOrdersNavigationRequestModel
            {
                Page = new SearchOrdersPageRequestModel { From = 0, Size = 10 },
                Sort = new SearchOrdersSortRequestModel()
            }
        };

        var query = _converter.ToSearchQuery(request);

        query.CustomerName.Should().BeNull();
        query.OrderDateFrom.Should().BeNull();
        query.OrderDateTo.Should().BeNull();
        query.PageSize.Should().Be(10);
    }

    [Fact]
    public void ToCreateCommand_MapsRequest()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var orderDate = new DateTime(2024, 3, 1);
        var request = Request(orderDate, customerId, productId);

        var command = _converter.ToCreateCommand(request);

        command.Order.Number.Should().Be("O-1");
        command.Order.OrderDate.Should().Be(orderDate);
        command.Order.Cost.Should().Be(250);
        command.Order.CostMethod.Should().Be(PaymentMethod.Cash);
        command.Order.Discount.Should().Be(5);
        command.Order.Profit.Should().Be(80);
        command.Order.Region.Should().Be("Kyivska");
        command.Order.City.Should().Be("Kyiv");
        command.Order.IsWholesale.Should().BeTrue();
        command.Order.Notes.Should().Be("note");
        command.Customer.Id.Should().Be(customerId);
        command.Customer.FirstName.Should().Be("Ann");
        command.Customer.LastName.Should().Be("Smith");
        command.Customer.Phone.Should().Be("123");
        command.Customer.Email.Should().Be("ann@example.com");
        command.Products.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(new { ProductId = productId, Quantity = 2, UnitPrice = 40m, CostPrice = 15m });
    }

    [Fact]
    public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
    {
        var id = Guid.NewGuid();
        var request = Request(new DateTime(2024, 4, 2), null, Guid.NewGuid());

        var command = _converter.ToUpdateCommand(id, request);

        command.Id.Should().Be(id);
        command.Order.Number.Should().Be("O-1");
        command.Products.Should().ContainSingle();
    }

    [Fact]
    public void ToCreateCommand_WhenProductsAreNull_ReturnsEmptyProducts()
    {
        var request = new OrderRequestModel
        {
            Number = "O-1",
            Customer = new OrderCustomerRequestModel { FirstName = "Ann", LastName = "Smith", Phone = "123" }
        };

        var command = _converter.ToCreateCommand(request);

        command.Products.Should().BeEmpty();
    }

    private static OrderRequestModel Request(DateTime orderDate, Guid? customerId, Guid productId) =>
        new OrderRequestModel
        {
            Number = "O-1",
            OrderDate = orderDate,
            Cost = 250,
            CostMethod = PaymentMethod.Cash,
            Discount = 5,
            Profit = 80,
            Region = "Kyivska",
            City = "Kyiv",
            IsWholesale = true,
            Notes = "note",
            Customer = new OrderCustomerRequestModel
            {
                Id = customerId,
                FirstName = "Ann",
                LastName = "Smith",
                Phone = "123",
                Email = "ann@example.com"
            },
            Products = new[]
            {
                new OrderProductRequestModel
                {
                    ProductId = productId,
                    Quantity = 2,
                    UnitPrice = 40,
                    CostPrice = 15
                }
            }
        };
}

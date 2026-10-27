using System;
using FluentAssertions;
using Mushka.Api.ClientModels.Supply;
using Mushka.Api.Converters.Supplies;
using Mushka.Domain.Entities;
using Xunit;

namespace Mushka.Api.Tests.Converters.Supplies
{
    public class SupplyRequestConverterTests
    {
        private readonly SupplyRequestConverter _converter = new SupplyRequestConverter();

        [Fact]
        public void ToCreateCommand_MapsRequest()
        {
            var supplierId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var requestDate = new DateTime(2024, 6, 1);
            var receivedDate = new DateTime(2024, 6, 2);
            var request = Request(supplierId, productId, requestDate, receivedDate);

            var command = _converter.ToCreateCommand(request);

            command.Supply.SupplierId.Should().Be(supplierId);
            command.Supply.RequestDate.Should().Be(requestDate);
            command.Supply.ReceivedDate.Should().Be(receivedDate);
            command.Supply.Description.Should().Be("winter");
            command.Supply.Prepayment.Should().Be(10);
            command.Supply.PrepaymentMethod.Should().Be(PaymentMethod.Cash);
            command.Supply.Cost.Should().Be(20);
            command.Supply.CostMethod.Should().Be(PaymentMethod.LiqPay);
            command.Supply.DeliveryCost.Should().Be(3);
            command.Supply.DeliveryCostMethod.Should().Be(PaymentMethod.TransferToCard);
            command.Supply.BankFee.Should().Be(1);
            command.Supply.TotalCost.Should().Be(24);
            command.Supply.Notes.Should().Be("note");
            command.Products.Should().ContainSingle()
                .Which.Should().BeEquivalentTo(new { ProductId = productId, Quantity = 4, UnitPrice = 5m, CostPrice = 6m });
        }

        [Fact]
        public void ToUpdateCommand_UsesRouteId_AndMapsRequest()
        {
            var id = Guid.NewGuid();
            var request = Request(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow);

            var command = _converter.ToUpdateCommand(id, request);

            command.Id.Should().Be(id);
            command.Supply.Description.Should().Be("winter");
            command.Products.Should().ContainSingle();
        }

        [Fact]
        public void ToCreateCommand_WhenProductsAreNull_ReturnsEmptyProducts()
        {
            var request = new SupplyRequestModel
            {
                Description = "winter",
                RequestDate = DateTime.UtcNow,
                ReceivedDate = DateTime.UtcNow
            };

            var command = _converter.ToCreateCommand(request);

            command.Products.Should().BeEmpty();
        }

        private static SupplyRequestModel Request(Guid supplierId, Guid productId, DateTime requestDate, DateTime receivedDate) =>
            new SupplyRequestModel
            {
                SupplierId = supplierId,
                RequestDate = requestDate,
                ReceivedDate = receivedDate,
                Description = "winter",
                Prepayment = 10,
                PrepaymentMethod = PaymentMethod.Cash,
                Cost = 20,
                CostMethod = PaymentMethod.LiqPay,
                DeliveryCost = 3,
                DeliveryCostMethod = PaymentMethod.TransferToCard,
                BankFee = 1,
                TotalCost = 24,
                Notes = "note",
                Products = new[]
                {
                    new SupplyProductRequestModel
                    {
                        ProductId = productId,
                        Quantity = 4,
                        UnitPrice = 5,
                        CostPrice = 6
                    }
                }
            };
    }
}

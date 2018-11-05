using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Delivery;

namespace Mushka.WebApi.Resolvers
{
    public class DeliveryConverter : ITypeConverter<Delivery, DeliveryModel>
    {
        public DeliveryModel Convert(Delivery source, DeliveryModel destination, ResolutionContext context) =>
            new DeliveryModel
            {
                Id = source.Id,
                RequestDate = source.RequestDate,
                DeliveryDate = source.DeliveryDate,
                PaymentMethod = source.PaymentMethod,
                Cost = source.Cost,
                TransferFee = source.TransferFee,
                Products = source.Products.Select(CreateDeliveryProductModel)
            };

        private static DeliveryProductModel CreateDeliveryProductModel(DeliveryProduct deliveryProduct) =>
            new DeliveryProductModel
            {
                ProductId = deliveryProduct.ProductId,
                ProductName = deliveryProduct.Product?.Name,
                SizeId = deliveryProduct.SizeId,
                SizeName = deliveryProduct.Size?.Name,
                Quantity = deliveryProduct.Quantity,
                PriceForItem = deliveryProduct.PriceForItem
            };
    }
}
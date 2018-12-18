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
                Supplier = source.Supplier.Name,
                RequestDate = source.RequestDate,
                ReceivedDate = source.DeliveryDate,
                Cost = source.Cost,
                TransferFee = source.TransferFee,
                BankFee = source.BankFee,
                TotalCost = source.Cost + source.TransferFee + source.BankFee,
                ProductsAmount = source.Products.Sum(prod => prod.ProductSizes.Sum(ps => ps.Quantity)),
                Products = source.Products.Select(CreateDeliveryProductModel)
            };

        private static DeliveryProductModel CreateDeliveryProductModel(DeliveryProduct deliveryProduct) =>
            new DeliveryProductModel
            {
                ProductId = deliveryProduct.ProductId,
                ProductName = deliveryProduct.Product?.Name,
                PriceForItem = deliveryProduct.PriceForItem,
                Sizes = deliveryProduct.ProductSizes.Select(CreateDeliveryProductSizeModel).ToList()
            };

        private static DeliveryProductSizeModel CreateDeliveryProductSizeModel(DeliveryProductSize deliveryProductSize) =>
            new DeliveryProductSizeModel
            {
                SizeId = deliveryProductSize.SizeId,
                Quantity = deliveryProductSize.Quantity
            };
    }
}
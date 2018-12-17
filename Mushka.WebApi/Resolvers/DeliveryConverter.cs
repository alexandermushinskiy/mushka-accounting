using System.Collections.Generic;
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
                Cost = source.Cost,
                TransferFee = source.TransferFee,
                Products = source.Products.Select(prod => CreateDeliveryProductModel(source.Products, prod))
            };

        private static DeliveryProductModel CreateDeliveryProductModel(IEnumerable<DeliveryProduct> allDeliveryProducts, DeliveryProduct deliveryProduct) =>
            new DeliveryProductModel
            {
                ProductId = deliveryProduct.ProductId,
                ProductName = deliveryProduct.Product?.Name,
                PriceForItem = deliveryProduct.PriceForItem,
                Sizes = allDeliveryProducts.Where(all => all.ProductId == deliveryProduct.ProductId).Select(CreateDeliveryProductSizeModel).ToList()
            };

        private static DeliveryProductSizeModel CreateDeliveryProductSizeModel(DeliveryProduct deliveryProduct) =>
            new DeliveryProductSizeModel
            {
                //SizeId = deliveryProduct.SizeId,
                //SizeName = deliveryProduct.Size?.Name,
                //Quantity = deliveryProduct.Quantity,
            };
    }
}
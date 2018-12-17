using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Delivery;

namespace Mushka.WebApi.Resolvers
{
    public class DeliveryRequestConverter : ITypeConverter<DeliveryRequestModel, Delivery>
    {
        private readonly IGuidProvider guidProvider;

        public DeliveryRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Delivery Convert(DeliveryRequestModel source, Delivery destination, ResolutionContext context)
        {
            var deliveryId = guidProvider.NewGuid();

            return new Delivery
            {
                Id = deliveryId,
                RequestDate = source.RequestDate,
                DeliveryDate = source.DeliveryDate,
                Cost = source.Cost,
                TransferFee = source.TransferFee,
                Products = source.Products.Select(prod => CreateDeliveryProduct(deliveryId, prod)).ToList()
            };
        }

        private static DeliveryProduct CreateDeliveryProduct(Guid deliveryId, DeliveryProductRequestModel requestModel)
        {
            return new DeliveryProduct
            {
                DeliveryId = deliveryId,
                ProductId = requestModel.ProductId,
                //SizeId = requestModel.SizeId,
                //Quantity = requestModel.Quantity,
                PriceForItem = requestModel.PriceForItem
            };
        }
    }
}
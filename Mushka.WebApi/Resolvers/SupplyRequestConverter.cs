using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supply;

namespace Mushka.WebApi.Resolvers
{
    public class SupplyRequestConverter : ITypeConverter<SupplyRequestModel, Supply>
    {
        private readonly IGuidProvider guidProvider;

        public SupplyRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Supply Convert(SupplyRequestModel source, Supply destination, ResolutionContext context)
        {
            var supplyId = guidProvider.NewGuid();

            return new Supply
            {
                Id = supplyId,
                RequestDate = source.RequestDate,
                ReceivedDate = source.ReceivedDate,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                DeliveryCost = source.DeliveryCost,
                DeliveryCostMethod = source.DeliveryCostMethod,
                BankFee = source.BankFee,
                Notes = source.Notes,
                Products = source.Products.Select(prod => CreateDeliveryProduct(supplyId, prod)).ToList()
            };
        }

        private static SupplyProduct CreateDeliveryProduct(Guid supplyId, SupplyProductRequestModel requestModel)
        {
            return new SupplyProduct
            {
                SupplyId = supplyId,
                ProductId = requestModel.ProductId,
                //SizeId = requestModel.SizeId,
                //Quantity = requestModel.Quantity,
                CostForItem = requestModel.CostPerItem
            };
        }
    }
}
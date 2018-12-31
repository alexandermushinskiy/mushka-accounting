using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supply;

namespace Mushka.WebApi.Resolvers
{
    public class SupplyConverter : ITypeConverter<Supply, SupplyModel>
    {
        public SupplyModel Convert(Supply source, SupplyModel destination, ResolutionContext context) =>
            new SupplyModel
            {
                Id = source.Id,
                SupplierName = source.Supplier.Name,
                RequestDate = source.RequestDate,
                ReceivedDate = source.ReceivedDate,
                Cost = source.Cost,
                CostMethod = source.CostMethod,
                Prepayment = source.Prepayment,
                PrepaymentMethod = source.PrepaymentMethod,
                BankFee = source.BankFee,
                DeliveryCost = source.DeliveryCost,
                DeliveryCostMethod = source.DeliveryCostMethod,
                TotalCost = source.TotalCost,
                ProductsAmount = source.Products.Sum(prod => prod.ProductSizes.Sum(ps => ps.Quantity)),
                Products = source.Products.Select(CreateDeliveryProductModel)
            };

        private static SupplyProductModel CreateDeliveryProductModel(SupplyProduct deliveryProduct) =>
            new SupplyProductModel
            {
                ProductId = deliveryProduct.ProductId,
                ProductName = deliveryProduct.Product?.Name,
                CostForItem = deliveryProduct.CostForItem,
                Sizes = deliveryProduct.ProductSizes.Select(CreateDeliveryProductSizeModel).ToList()
            };

        private static SupplyProductSizeModel CreateDeliveryProductSizeModel(SupplyProductSize deliveryProductSize) =>
            new SupplyProductSizeModel
            {
                SizeId = deliveryProductSize.SizeId,
                Quantity = deliveryProductSize.Quantity
            };
    }
}
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supply.Describe;

namespace Mushka.WebApi.Resolvers.Supplies
{
    public class DescribeSupplyResponseConverter : ITypeConverter<OperationResult<Supply>, DescribeSupplyResponseModel>
    {
        public DescribeSupplyResponseModel Convert(
            OperationResult<Supply> source,
            DescribeSupplyResponseModel destination,
            ResolutionContext context)
        {
            return new DescribeSupplyResponseModel
            {
                Supply = ConvertToSupply(source.Data),
                Products = source.Data.Products.Select(ConvertToSupplyProduct)
            };
        }

        private static SupplyModel ConvertToSupply(Supply source) =>
            new SupplyModel
            {
                Id = source.Id,
                SupplierId = source.SupplierId,
                SupplierName = source.Supplier?.Name,
                Description = source.Description,
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
                ProductsAmount = source.Products.Sum(prod => prod.Quantity),
                Notes = source.Notes
            };

        private static SupplyProductModel ConvertToSupplyProduct(SupplyProduct supplyProduct) =>
            new SupplyProductModel
            {
                UnitPrice = supplyProduct.UnitPrice,
                Quantity = supplyProduct.Quantity,
                CostPrice = supplyProduct.CostPrice,
                Product = new ProductModel
                {
                    Id = supplyProduct.ProductId,
                    Name = supplyProduct.Product?.Name,
                    VendorCode = supplyProduct.Product?.VendorCode,
                    SizeName = supplyProduct.Product?.Size?.Name
                }
            };
    }
}
﻿using System.Linq;
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
                SupplierId = source.SupplierId,
                SupplierName = source.Supplier?.Name,
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
                Products = source.Products.Select(CreateDeliveryProductModel),
                Notes = source.Notes
            };

        private static SupplyProductModel CreateDeliveryProductModel(SupplyProduct supplyProduct) =>
            new SupplyProductModel
            {
                CostForItem = supplyProduct.CostForItem,
                Quantity = supplyProduct.Quantity,
                Product = new ProductModel
                {
                    Id = supplyProduct.ProductId,
                    Name = supplyProduct.Product?.Name,
                    VendorCode = supplyProduct.Product?.VendorCode,
                    Size = supplyProduct.Product?.Size?.Name
                }
            };
    }
}
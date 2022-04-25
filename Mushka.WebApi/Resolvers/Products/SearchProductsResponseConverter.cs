using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers.Products
{
    public class SearchProductsResponseConverter : ITypeConverter<OperationResult<IEnumerable<Product>>, SearchProductsResponseModel>
    {
        public SearchProductsResponseModel Convert(
            OperationResult<IEnumerable<Product>> source,
            SearchProductsResponseModel destination,
            ResolutionContext context)
        {
            return new SearchProductsResponseModel
            {
                Total = source.Data?.Count() ?? 0,
                Items = source.Data?.Select(ConvertToProductSummaryModel) ?? Enumerable.Empty<ProductSummaryModel>()
            };
        }

        private static ProductSummaryModel ConvertToProductSummaryModel(Product product)
        {
            var lastSupply = product.Supplies
                .OrderByDescending(del => del.Supply.ReceivedDate)
                .Select(del => del.Supply)
                .FirstOrDefault();

            var lastSupplyCount = lastSupply?.Products
                .Where(del => del.ProductId == product.Id)
                .Select(prod => prod.Quantity).Single();

            return new ProductSummaryModel
            {
                Id = product.Id,
                Name = product.Name,
                VendorCode = product.VendorCode,
                RecommendedPrice = product.RecommendedPrice,
                CreatedOn = product.CreatedOn,
                Quantity = product.Quantity,
                DeliveriesCount = product.Supplies.Count,
                LastDeliveryDate = lastSupply?.ReceivedDate,
                LastDeliveryCount = lastSupplyCount,
                SizeName = product.Size?.Name
            };
        }
    }
}
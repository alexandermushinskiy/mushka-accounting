using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers.Products
{
    public class SelectProductsResponseConverter : ITypeConverter<OperationResult<IEnumerable<Product>>, SelectProductsResponseModel>
    {
        public SelectProductsResponseModel Convert(
            OperationResult<IEnumerable<Product>> source,
            SelectProductsResponseModel destination,
            ResolutionContext context)
        {
            var items = ConvertToSelectProductModels(source.Data);

            return new SelectProductsResponseModel
            {
                Total = items.Count,
                Items = items
            };
        }

        private IReadOnlyList<SelectProductModel> ConvertToSelectProductModels(IEnumerable<Product> products)
            => products.Select(product => new SelectProductModel
            {
                Id = product.Id,
                Name = product.Name,
                VendorCode = product.VendorCode,
                RecommendedPrice = product.RecommendedPrice,
                Quantity = product.Quantity,
                CategoryName = product.Category.Name,
                SizeName = product.Size?.Name,
                IsArchival = product.IsArchival
            }).ToList();
    }
}
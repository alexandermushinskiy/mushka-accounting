using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductConverter : ITypeConverter<Product, ProductModel>
    {
        public ProductModel Convert(Product source, ProductModel destination, ResolutionContext context)
        {
            var lastSupply = source.Supplies
                .OrderByDescending(del => del.Supply.ReceivedDate)
                .Select(del => del.Supply)
                .FirstOrDefault();

            var lastSupplyCount = lastSupply?.Products
                .Where(del => del.ProductId == source.Id)
                .Select(prod => prod.ProductSizes.Sum(ps => ps.Quantity))
                .Single();
            
            return new ProductModel
            {
                Id = source.Id,
                Name = source.Name,
                Code = source.Code,
                CreatedOn = source.CreatedOn,
                CategoryId = source.CategoryId,
                Category = ConvertToCategoryModel(source.Category),
                DeliveriesCount = source.Supplies.Count,
                LastDeliveryDate = lastSupply?.ReceivedDate,
                LastDeliveryCount = lastSupplyCount,
                Sizes = source.Sizes.Select(CreateProductSizeModel).ToArray()
            };
        }

        private static CategoryModel ConvertToCategoryModel(Category category)
        {
            return category == null ? null : new CategoryModel { Id = category.Id, Name = category.Name };
        }

        private static ProductSizeModel CreateProductSizeModel(ProductSize productSize) =>
            new ProductSizeModel
            {
                Id = productSize.SizeId,
                Name = productSize.Size.Name,
                Quantity = productSize.Quantity
            };
    }
}
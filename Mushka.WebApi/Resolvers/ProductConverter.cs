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
                .Select(prod => prod.Quantity).Single();

            var size = source.Size == null ? null : new SizeModel { Id = source.Size.Id, Name = source.Size.Name };

            return new ProductModel
            {
                Id = source.Id,
                Name = source.Name,
                VendorCode = source.VendorCode,
                CreatedOn = source.CreatedOn,
                CategoryId = source.CategoryId,
                Quantity = source.Quantity,
                Category = ConvertToCategoryModel(source.Category),
                DeliveriesCount = source.Supplies.Count,
                LastDeliveryDate = lastSupply?.ReceivedDate,
                LastDeliveryCount = lastSupplyCount,
                Size = size
            };
        }

        private static CategoryModel ConvertToCategoryModel(Category category) =>
            category == null ? null : new CategoryModel { Id = category.Id, Name = category.Name };
    }
}
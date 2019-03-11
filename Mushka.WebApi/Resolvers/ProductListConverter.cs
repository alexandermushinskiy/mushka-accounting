using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductListConverter : ITypeConverter<Product, ProductListModel>
    {
        public ProductListModel Convert(Product source, ProductListModel destination, ResolutionContext context)
        {
            var lastSupply = source.Supplies
                .OrderByDescending(del => del.Supply.ReceivedDate)
                .Select(del => del.Supply)
                .FirstOrDefault();

            var lastSupplyCount = lastSupply?.Products
                .Where(del => del.ProductId == source.Id)
                .Select(prod => prod.Quantity).Single();
            
            return new ProductListModel
            {
                Id = source.Id,
                Name = source.Name,
                VendorCode = source.VendorCode,
                RecommendedPrice = source.RecommendedPrice,
                CreatedOn = source.CreatedOn,
                Quantity = source.Quantity,
                DeliveriesCount = source.Supplies.Count,
                LastDeliveryDate = lastSupply?.ReceivedDate,
                LastDeliveryCount = lastSupplyCount,
                SizeName = source.Size?.Name
            };
        }
    }
}
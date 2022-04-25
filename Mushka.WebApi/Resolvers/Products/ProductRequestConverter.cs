using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers
{
    public class ProductRequestConverter : ITypeConverter<ProductRequestModel, Product>
    {
        private readonly IGuidProvider guidProvider;
        private readonly IDateTimeProvider dateTimeProvider;

        public ProductRequestConverter(
            IGuidProvider guidProvider,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidProvider = guidProvider;
            this.dateTimeProvider = dateTimeProvider;
        }

        public Product Convert(ProductRequestModel source, Product destination, ResolutionContext context)
        {
            var productId = context.GetId() ?? guidProvider.NewGuid();

            return new Product
            {
                Id = productId,
                CategoryId = source.CategoryId,
                CreatedOn = dateTimeProvider.GetNow(),
                Name = source.Name,
                VendorCode = source.VendorCode,
                RecommendedPrice = source.RecommendedPrice,
                SizeId = source.SizeId,
                IsArchival = source.IsArchival
            };
        }
    }
}
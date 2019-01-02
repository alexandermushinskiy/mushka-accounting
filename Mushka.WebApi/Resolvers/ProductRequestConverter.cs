using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

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
            var produvtId = guidProvider.NewGuid();

            return new Product
            {
                Id = produvtId,
                CategoryId = source.CategoryId,
                CreatedOn = dateTimeProvider.GetNow(),
                Name = source.Name,
                VendorCode = source.VendorCode,
                SizeId = source.SizeId
            };
        }
    }
}
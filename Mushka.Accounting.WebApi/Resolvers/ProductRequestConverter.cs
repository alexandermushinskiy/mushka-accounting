using System.Linq;
using AutoMapper;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels.Product;

namespace Mushka.Accounting.WebApi.Resolvers
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
                CreatedOn = dateTimeProvider.GetNow(),
                Name = source.Name,
                Code = source.Code,
                Sizes = source.Sizes.Select(sizeId => new ProductSize { ProductId = produvtId, SizeId = sizeId } ).ToList()
            };
        }
    }
}
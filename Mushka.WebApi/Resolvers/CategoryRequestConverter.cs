using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category;

namespace Mushka.WebApi.Resolvers
{
    public class CategoryRequestConverter : ITypeConverter<CategoryRequestModel, Category>
    {
        private readonly IGuidProvider guidProvider;

        public CategoryRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Category Convert(CategoryRequestModel source, Category destination, ResolutionContext context)
        {
            return new Category
            {
                Id = guidProvider.NewGuid(),
                Name = source.Name,
                IsSizeRequired = source.IsSizeRequired
            };
        }
    }
}
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category;

namespace Mushka.WebApi.Resolvers
{
    public class CategoryRequestResolver : ITypeConverter<CategoryRequestModel, Category>
    {
        private readonly IGuidProvider guidProvider;

        public CategoryRequestResolver(IGuidProvider guidProvider)
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
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category;

namespace Mushka.WebApi.Resolvers
{
    public class CategoryConverter : ITypeConverter<Category, CategoryModel>
    {
        public CategoryModel Convert(Category source, CategoryModel destination, ResolutionContext context) =>
            new CategoryModel
            {
                Id = source.Id,
                Name = source.Name,
                IsSizeRequired = source.IsSizeRequired,
                IsAdditional = source.IsAdditional
            };
    }
}